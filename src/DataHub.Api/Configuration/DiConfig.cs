using DataHub.Server.Configuration;

namespace DataHub.Api.Configuration;

/// <summary>
/// Provides extension methods for configuring dependency injection.
/// </summary>
public static class DiConfig
{
    /// <summary>
    /// Adds configuration for custom services to the specified <see cref="IServiceCollection"/>.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to add the services to.</param>
    /// <param name="configuration">The <see cref="IConfiguration"/> from which to retrieve configuration settings.</param>
    /// <returns>The <see cref="IServiceCollection"/> instance with the configured services added.</returns>
    public static IServiceCollection AddCustomServices(this IServiceCollection services, IConfiguration configuration)
        => services.AddExternalApis(configuration);
}