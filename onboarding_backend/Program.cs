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

    try
    {
        // Les filen til en MemoryStream
        using var memoryStream = new MemoryStream();
        await file.CopyToAsync(memoryStream);
        memoryStream.Position = 0;

        // Last inn XML direkte fra minnet
        var doc = XDocument.Load(memoryStream);

        // Flat ut XML-strukturen
        var results = SafTFlattener.FlattenSafTAsList(doc.Root!);

        // Konverter til JSON og returner til klienten
        return Results.Json(results);
    }
    catch (Exception ex)
    {
        Console.WriteLine("Error processing file: " + ex.Message);
        return Results.Problem("Failed to process the uploaded file.");
    }
});


app.Run();
