using System.Xml.Linq;
using onboarding_backend.Models;
using onboarding_backend.Services;
using System.Xml.Serialization;
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

    AuditFile saft;
    try
    {
        var serializer = new XmlSerializer(typeof(AuditFile));
        using (var reader = new FileStream(filePath, FileMode.Open))
        {
            saft = (AuditFile)serializer.Deserialize(reader)!;
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error parsing XML: {ex.Message}");
        return Results.BadRequest("Failed to parse the XML file");
    }

    var (customers, suppliers, contacts) = Converter.ConvertFromSaft(saft);

    var doc = new XDocument(
        new XElement("PowerOfficeImport",
            new XElement("Customer",
                customers.Select(c =>
                    new XElement("Customer",
                        new XElement("CustomerNo", c.CustomerNo),
                        new XElement("Name", c.Name),
                        new XElement("Phone", c.Phone),
                        new XElement("Email", c.Email),
                        new XElement("OrganizationNo", c.OrganizationNo)
                    )
                )
            ),
        new XElement("PowerOfficeImport",
            new XElement("Suppliers",
                suppliers.Select(c =>
                    new XElement("Supplier",
                        new XElement("SupplierNO", c.SupplierNo),
                        new XElement("Name", c.Name),
                        new XElement("Phone", c.Phone),
                        new XElement("Email", c.Email),
                        new XElement("OrganizationNo", c.OrganizationNo)
                        )
                    )
                )
            )
        )
    );

    var filteredPath = Path.Combine(uploads, "New_SAF-T.xml");
    doc.Save(filteredPath);


    return Results.Ok(new { message = "File uploaded successfully", filteredFile = filteredPath });
});

app.Run();