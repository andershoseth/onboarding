using System.Text.Json;
using Microsoft.AspNetCore.Http.Features;
using onboarding_backend; // Inneholder SaftParser og StandardImport

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalhostAllPorts", policy =>
    {
<<<<<<< HEAD
        policy.WithOrigins("http://localhost:3000")
              .AllowAnyMethod()
              .AllowAnyHeader();
=======
        policy
            .SetIsOriginAllowed(origin =>
            {
                var uri = new Uri(origin);
                return uri.IsLoopback;
            })
            .AllowAnyMethod()
            .AllowAnyHeader();
>>>>>>> origin/main
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

app.Run();
