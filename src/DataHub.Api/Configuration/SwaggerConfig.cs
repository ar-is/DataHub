using System.Reflection;
using DataHub.Core.Models;
using DataHub.Server.Apis;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace DataHub.Api.Configuration;

/// <summary>
/// Provides extension methods for setting up Swagger Gen services in an <see cref="IServiceCollection"/>.
/// </summary>
public static class SwaggerConfig
{
    /// <summary>
    /// Adds Swagger generator services to the specified <see cref="WebApplicationBuilder"/>.
    /// </summary>
    /// <param name="builder">The <see cref="WebApplicationBuilder"/> to add services</param>
    /// <returns> An <see cref="WebApplicationBuilder"/> that can be used to further configure dependencies.</returns>
    public static IServiceCollection AddSwaggerGenConfig(this WebApplicationBuilder builder)
    {
        return builder.Services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo { Title = "DataHub API", Version = "v1" });
            options.IncludeXmlComments(Assembly.GetExecutingAssembly());
            options.IncludeXmlComments(typeof(AggregatedData).Assembly);
            options.IncludeXmlComments(typeof(DataAggregationApi).Assembly);
        });
    }

    /// <summary>
    /// Configures Swagger and Swagger UI for extended API documentation.
    /// </summary>
    /// <param name="app">The <see cref="WebApplication"/> to configure.</param>
    /// <returns>The same <see cref="IApplicationBuilder"/> for method chaining.</returns>
    public static IApplicationBuilder UseSwaggerExtended(this WebApplication app)
    {
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "DataHub API");
        });

        return app;
    }

    /// <summary>
    /// Includes XML comments from an external assembly. Useful when models are located in more than one assembly.
    /// </summary>
    /// <param name="options">The options to configure.</param>
    /// <param name="assembly">The assembly to scan for XML comments.</param>
    private static void IncludeXmlComments(this SwaggerGenOptions options, Assembly assembly)
    {
        var text = Path.Combine(AppContext.BaseDirectory, assembly.GetName().Name + ".xml");
        if (File.Exists(text))
        {
            options.IncludeXmlComments(text);
        }
    }
}