using System.Text.Json;
using System.Xml.Linq;
using Microsoft.AspNetCore.Http.Features;
using onboarding_backend;
using onboarding_backend.Services; // Inneholder SaftParser og StandardImport
using System.Text;
using System.Collections.Concurrent;
using Microsoft.OpenApi.Models; // For OpenAPI/Swagger

var builder = WebApplication.CreateBuilder(args);

// 1) Add Services for Controllers
builder.Services.AddControllers();

// 2) Add CORS policy (as you already did)
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

// 3) Add Endpoint Explorer and SwaggerGen
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Onboarding API",
        Version = "v1",
        Description = "Sample endpoints for file processing"
    });
});

var mappedCsvStore = new ConcurrentDictionary<string, byte[]>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Onboarding API V1");
    c.RoutePrefix = ""; // Swagger UI at root
});

// Continue with the rest of your pipeline:
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

    return Results.Json(groupedFields);
});

app.MapPost("/api/perform-mapping", (MappingRequest request) =>
{
    // request.Mapping = dictionary: { "Name": "CustomerTable.NameField", "Email": "CustomerTable.Email" }
    // request.Data = the list of CSV rows: e.g. [ { "Name": "Alice", "Email": "alice@example.com" }, ...]

    var mapping = request.Mapping;
    var rows = request.Data;

    // 1) Transform the CSV rows based on user’s mapping
    var transformedRows = new List<Dictionary<string, string>>();
    foreach (var row in rows)
    {
        var newRow = new Dictionary<string, string>();
        foreach (var (columnName, value) in row)
        {
            // If the user mapped "columnName" => "someTable.someField", store it under that key
            if (mapping.ContainsKey(columnName))
            {
                var mappedField = mapping[columnName];
                newRow[mappedField] = value;
            }
            else
            {
                // Keep the original column and value
                newRow[columnName] = value;
            }
        }
        transformedRows.Add(newRow);
    }

    // 2) Generate CSV from transformedRows
    var allKeys = transformedRows
        .SelectMany(dict => dict.Keys)
        .Distinct()
        .ToList();

    var sb = new StringBuilder();

    // Write the header row
    var headerRow = allKeys.Select(col =>
{
    if (mapping.TryGetValue(col, out var mappedName))
    {
        return mappedName; // mapped field name
    }
    return col; // fallback to original column
});
    sb.AppendLine(string.Join(",", headerRow));

    // Write each data row
    foreach (var dict in transformedRows)
    {
        var cells = allKeys.Select(k => dict.ContainsKey(k) ? dict[k] : "");
        // Simple "quote any double quotes" approach
        var escaped = cells.Select(c => $"\"{c?.Replace("\"", "\"\"")}\"");
        sb.AppendLine(string.Join(",", escaped));
    }

    // Convert to byte[] for storage
    var csvBytes = Encoding.UTF8.GetBytes(sb.ToString());

    // Store in a dictionary with a unique ID
    var id = Guid.NewGuid().ToString("N");
    mappedCsvStore[id] = csvBytes;

    // Return { success=true, id=theID }
    return Results.Ok(new { success = true, id });
});

app.MapGet("/api/download/{id}", (string id) =>
{
    if (mappedCsvStore.TryGetValue(id, out var csvBytes))
    {
        // Return a CSV file with name 'mapped.csv'
        return Results.File(csvBytes, "text/csv", "mapped.csv");
    }
    return Results.NotFound("CSV not found or already removed.");
});




app.MapControllers();
app.Run();
