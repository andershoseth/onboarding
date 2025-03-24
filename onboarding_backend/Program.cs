using System.Text.Json;
using System.Xml.Linq;
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


app.MapPost("/api/upload", async (HttpRequest request) =>
{
    if (!request.HasFormContentType)
    {
        return Results.BadRequest("Unsupported content type.");
    }

    var form = await request.ReadFormAsync();

    // GET the subject from the form data
    var subject = form["subject"].ToString();

    var file = form.Files["file"];
    if (file == null || file.Length == 0)
    {
        return Results.BadRequest("No file uploaded.");
    }

    try
    {
        var fileExtension = Path.GetExtension(file.FileName).ToLower();

        using var memoryStream = new MemoryStream();
        await file.CopyToAsync(memoryStream);
        memoryStream.Position = 0;

        object results;

        if (fileExtension == ".xml")
        {
            var doc = XDocument.Load(memoryStream);


            var flattened = SafTFlattener.FlattenSafTAsList(doc.Root!);


            var grouped = SafTFlattener.GroupSafTEntries(flattened);
            var filteredGroups = grouped
                .Where(g => g.GroupKey == "AuditFile.Header"
                         || g.GroupKey == "AuditFile.MasterFiles")
                .ToList();


            results = filteredGroups;
        }
        else if (fileExtension == ".csv")
        {
            results = FileProcessor.ProcessCsv(memoryStream);
        }
        else if (fileExtension == ".xlsx")
        {
            results = FileProcessor.ProcessExcel(memoryStream);
        }
        else
        {
            return Results.BadRequest("Unsupported filetype.");
        }

        // Return object containing both subject & the processed data:
        // Console.WriteLine(JsonSerializer.Serialize(results));
        return Results.Json(new
        {
            subject,  // e.g. "kontakter", "saldobalanse", etc.
            data = results // Processed data
        });
    }
    catch (Exception ex)
    {
        Console.WriteLine("Error processing file: " + ex.Message);
        return Results.Problem("Failed to process the uploaded file.");
    }
});

app.MapGet("/api/standard-import-mapping", () =>
{
    var groupedFields = FieldMappingHelper.GetStandardImportGroupedFields();

    Console.WriteLine("Mapping for StandardImport:");
    foreach (var tableMapping in groupedFields)
    {
        Console.WriteLine($"Table: {tableMapping.TableName}");
        foreach (var field in tableMapping.Fields)
        {
            Console.WriteLine($"  {field.Field}");
        }
    }

    return Results.Json(groupedFields);
});


app.Run();
