using Microsoft.AspNetCore.Http.Features;

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

    var nameForFile = "Name";
    try
    {
        var xdoc = System.Xml.Linq.XDocument.Load(filePath);

        var transactionIds = xdoc.Descendants().Where(e => e.Name.LocalName == nameForFile).Select(e => e.Value);

        var transactionsList = new List<string>(); // list to display total of data extracted
        foreach (var id in transactionIds)
        {
            Console.WriteLine($"{nameForFile}: {id}");
            transactionsList.Add(id);
        }

        Console.WriteLine($"Total: {transactionIds.Count()}"); // print sum
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error parsing XML: {ex.Message}");
        return Results.BadRequest("Failed to parse the XML file");
    }

    return Results.Ok(new { message = "File uploaded successfully", fileName = file.FileName });
});

app.Run();