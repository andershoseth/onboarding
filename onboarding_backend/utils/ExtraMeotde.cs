using System.Text.Json;
using System.Xml.Linq;
using Microsoft.AspNetCore.Http.Features;
using onboarding_backend;
using onboarding_backend.Services; // Inneholder SaftParser og StandardImport

public static class ExtraMetode
{
    public static void ConfigureCors(IServiceCollection services)
    {
        services.AddCors(options =>
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
    }

    public static void ConfigureMaxRequestBodySize(IApplicationBuilder app)
    {
        app.Use(async (context, next) =>
        {
            var maxRequestBodySizeFeature = context.Features.Get<IHttpMaxRequestBodySizeFeature>();
            if (maxRequestBodySizeFeature != null)
            {
                maxRequestBodySizeFeature.MaxRequestBodySize = 104857600; // 100 MB
            }
            await next.Invoke();
        });
    }

    public static void MapGetFlattened(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/api/getflattened", async (HttpRequest request) =>
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
    }

    public static void MapGetNested(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/api/getnested", async (HttpRequest request) =>
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
    }
    }