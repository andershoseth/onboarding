using System.Text.Json;
using Microsoft.AspNetCore.Http.Features;
using onboarding_backend;
using onboarding_backend.Services; // Inneholder SaftParser og StandardImport

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalhostAllPorts", policy =>
    {

        policy
            .SetIsOriginAllowed(origin =>
            {
                var uri = new Uri(origin);
                return uri.IsLoopback;
            })
            .AllowAnyMethod()
            .AllowAnyHeader();

    });
});

var app = builder.Build();
app.UseCors("AllowLocalhostAllPorts");

// Øk maks request-body størrelse (f.eks. 100MB)
app.Use(async (context, next) =>
{
    var maxRequestBodySizeFeature = context.Features.Get<IHttpMaxRequestBodySizeFeature>();
    if (maxRequestBodySizeFeature != null)
    {
        maxRequestBodySizeFeature.MaxRequestBodySize = 104857600; // 100 MB
    }
    await next.Invoke();
});

// API-endepunkt for filopplasting
app.MapPost("/api/upload", async (HttpRequest request) =>
{
    if (!request.HasFormContentType)
    {
        return Results.BadRequest("Unsupported content type.");
    }

    var form = await request.ReadFormAsync();
    var file = form.Files["file"];

    if (file == null || file.Length == 0)
    {
        return Results.BadRequest("No file uploaded.");
    }

    var saftParser = new SaftParser();
    // Kaller parseren -> får StandardImport-objekt
    StandardImport stdImport = await saftParser.ProcessSaftFileAsync(file);

    // Du kan sjekke om vi fant noen data (f.eks. kontakter, bilag, osv.)
    bool hasData = (stdImport.Contacts.Any() ||
                    stdImport.Products.Any() ||
                    stdImport.Projects.Any() ||
                    stdImport.ProjectTeamMembers.Any() ||
                    stdImport.ProjectActivities.Any() ||
                    stdImport.Departments.Any() ||
                    stdImport.Vouchers.Any());

    if (!hasData)
    {
        return Results.BadRequest(new { error = "No valid data found in XML file." });
    }

    // Skriv ut alt til terminalen som JSON (valgfritt)
    Console.WriteLine("\n🔹 **Final Parsed Data (StandardImport):**");
    Console.WriteLine(JsonSerializer.Serialize(stdImport, new JsonSerializerOptions { WriteIndented = true }));

    // Returner data i HTTP-responsen
    return Results.Ok(new
    {
        message = "File uploaded and processed successfully",
        fileName = file.FileName,
        standardImport = stdImport
    });
});

app.MapGet("/api/getflattened", async (HttpRequest request) =>
{
    // Hardcoded file path for testing
    string relativePath = Path.Combine("..", "..", "..", "examplefiles", "saft_examplefile.xml");
    string filePath = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, relativePath));

    try
    {
        var dictResult = SafTFlattener.FlattenSafTAsDictionary(filePath);

        // You can serialize a list the same way
        var options = new JsonSerializerOptions { WriteIndented = true };
        var jsonOutput = JsonSerializer.Serialize(dictResult, options);

        // Example console output
        foreach (var kvp in dictResult)
        {
            Console.WriteLine($"Path = {kvp.Key}, Value = {kvp.Value}");
        }

        return jsonOutput;
    }
    catch (Exception ex)
    {
        Console.WriteLine("An error occurred while flattening the file: " + ex.Message);
        throw;
    }
});



app.MapGet("/api/getflattenedlist", async (HttpRequest request) =>
{
    // Hardcoded file path for testing
    string relativePath = Path.Combine("..", "..", "..", "examplefiles", "saft_examplefile.xml");
    string filePath = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, relativePath));

    try
    {
        var listResult = SafTFlattener.FlattenSafTAsList(filePath);

        // You can serialize a list the same way
        var options = new JsonSerializerOptions { WriteIndented = true };
        var jsonOutput = JsonSerializer.Serialize(listResult, options);

        // Example console output
        foreach (var entry in listResult)
        {
            Console.WriteLine($"Path = {entry.Path}, Value = {entry.Value}");
        }
        
        Console.WriteLine(listResult);
        return jsonOutput;
    }
    catch (Exception ex)
    {
        Console.WriteLine("An error occurred while flattening the file: " + ex.Message);
        throw;
    }
});

app.MapGet("/api/getnested", async (HttpRequest request) =>
{
    // Hardcoded file path for testing
    string relativePath = Path.Combine("..", "..", "..", "examplefiles", "saft_examplefile.xml");
    string filePath = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, relativePath));

    try
    {
        // Call our nested structure method
        var nestedData = SafTNestedFlattener.FlattenSafTAsNested(filePath);

        // Convert to indented JSON string
        var options = new JsonSerializerOptions { WriteIndented = true };
        string jsonOutput = JsonSerializer.Serialize(nestedData, options);

        // Return as JSON
        return Results.Content(jsonOutput, "application/json");
    }
    catch (Exception ex)
    {
        Console.WriteLine("An error occurred while reading the file: " + ex.Message);
        return Results.Problem(ex.Message);
    }
});

app.Run();
