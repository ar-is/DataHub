using DataHub.Api.Configuration;
using DataHub.Server.Apis;

namespace DataHub.Api;

/// <summary>
/// The main entry point for the application.
/// </summary>
public class Program
{
    /// <summary>
    /// The entry point method of the application.
    /// </summary>
    /// <param name="args">The command-line arguments.</param>
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.AddSwaggerGenConfig();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddCustomServices(builder.Configuration);
        builder.Services.AddAuthenticationConfig(builder.Configuration);
        builder.Services.AddAuthorization();
        builder.Services.AddDistributedMemoryCache();

        var app = builder.Build();

        app.UseHttpsRedirection();
        app.UseSwaggerExtended();

        app.MapAuthentication();
        app.MapDataAggregation();

        app.UseAuthentication();
        app.UseAuthorization();

        app.Run();
    }
}