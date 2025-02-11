using Microsoft.AspNetCore.Http.Features;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowNextJs", policy =>
    {
        policy.WithOrigins("http://localhost:3000")
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

var app = builder.Build();
app.UseCors("AllowNextJs");

app.Use(async (context, next) =>
{
    context.Features.Get<IHttpMaxRequestBodySizeFeature>()!.MaxRequestBodySize = 104857600; // e.g., 100MB
    await next.Invoke();
});

app.MapPost("/api/upload", async (HttpRequest request) =>
{
    if (!request.HasFormContentType)
    {
        return Results.BadRequest("Unsupported content type.");
    }

    var form = await request.ReadFormAsync();
    var file = form.Files["file"]; // ensure your form data key is "file"

    if (file == null || file.Length == 0)
    {
        return Results.BadRequest("No file uploaded.");
    }

    // Define where to store the file temporarily (or process it immediately)
    var uploads = Path.Combine(Directory.GetCurrentDirectory(), "uploads");
    Directory.CreateDirectory(uploads);
    var filePath = Path.Combine(uploads, file.FileName);

    // Save the file locally
    using (var stream = new FileStream(filePath, FileMode.Create))
    {
        await file.CopyToAsync(stream);
    }

    // Here you might parse the file based on its extension (.csv, .xlsx, .saf-t)
    // For example:
    // if (Path.GetExtension(file.FileName).Equals(".csv", StringComparison.OrdinalIgnoreCase))
    // {
    //     // Use CsvHelper or similar to process CSV files.
    // }

    return Results.Ok(new { message = "File uploaded successfully", fileName = file.FileName });
});

app.Run();