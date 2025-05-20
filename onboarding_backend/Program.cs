using System.Xml.Linq;
using Microsoft.AspNetCore.Http.Features;
using onboarding_backend;
using onboarding_backend.Services; // Inneholder SaftParser og StandardImport
using System.Text;
using System.Collections.Concurrent;
using Microsoft.OpenApi.Models; // For OpenAPI/Swagger
using onboarding_backend.Models;
using onboarding_backend.Models.StandardImport;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

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

// endpoint explorer and swagger gen
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
    c.RoutePrefix = "swagger"; // Swagger UI at root
});

// Continuing the pipeline
app.UseCors("AllowLocalhostAllPorts");


// Setting size limit for request body size. Can be changed
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

            var filteredgrouped = grouped
            .Where(g => g.GroupKey.Contains("Transaction"))
            .ToList();



            // Combining transaction groups 
            var combinedTransactions = new GroupedSafTEntries
            {
                GroupKey = "Transactions",
                Entries = filteredgrouped.SelectMany(g => g.Entries).ToList()
            };

            results = new List<GroupedSafTEntries> { combinedTransactions };


            ;
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

    var mapping = request.Mapping;
    var rows = request.Data;

    // transforming csv rows based on mapping
    var transformedRows = new List<Dictionary<string, string>>();
    foreach (var row in rows)
    {
        var newRow = new Dictionary<string, string>();
        foreach (var (columnName, value) in row)
        {
            // storing data under new mapped column key
            if (mapping.ContainsKey(columnName))
            {
                var mappedField = mapping[columnName];
                newRow[mappedField] = value;
            }
            else
            {
                // but keeep the original column and value
                newRow[columnName] = value;
            }
        }
        transformedRows.Add(newRow);
    }

    // generating the csv from transformedrows
    var allKeys = transformedRows
        .SelectMany(dict => dict.Keys)
        .Distinct()
        .ToList();

    var sb = new StringBuilder();

    var headerRow = allKeys.Select(col =>
{
    if (mapping.TryGetValue(col, out var mappedName))
    {
        return mappedName; // mapped FIELD name
    }
    return col; // falling back to original column in event of no mapping
});
    sb.AppendLine(string.Join(",", headerRow));

    // Writing each data row
    foreach (var dict in transformedRows)
    {
        var cells = allKeys.Select(k => dict.ContainsKey(k) ? dict[k] : "");
        // quoting any double quotes
        var escaped = cells.Select(c => $"\"{c?.Replace("\"", "\"\"")}\"");
        sb.AppendLine(string.Join(",", escaped));
    }

    // Converting to byte[] for storage
    var csvBytes = Encoding.UTF8.GetBytes(sb.ToString());

    // Storing in a dictionary with a unique ID
    var id = Guid.NewGuid().ToString("N");
    mappedCsvStore[id] = csvBytes;

    return Results.Ok(new { success = true, id });
});

app.MapGet("/api/download/{id}", (string id) =>
{
    if (mappedCsvStore.TryGetValue(id, out var csvBytes))
    {
        // we can change filename later
        return Results.File(csvBytes, "text/csv", "mapped.csv");
    }
    return Results.NotFound("CSV not found or already removed.");
});


app.MapPost("/api/standard-import-object", (Standardimport stdImport) =>
    {

        if (stdImport == null)
            return Results.BadRequest("No data provided.");

        // Generate .xlsx in memory
        byte[] fileContents = ExcelSingleSheetExporter.CreateSingleSheet(stdImport);

        // Returning as downloadable file
        return Results.File(
            fileContents,
            contentType: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            fileDownloadName: "StandardImport.xlsx"
        );
    });






app.MapControllers();
app.Run();
