using DataHub.Core.Abstractions;
using DataHub.Core.Configuration.Options;
using DataHub.Core.Models;
using DataHub.Server.Clients;
using DataHub.Server.Configuration.Options;
using DataHub.Server.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DataHub.Server.Configuration;

/// <summary>
/// Provides extension methods for configuring dependency injection.
/// </summary>
public static class DiConfig
{
    /// <summary>
    /// Adds configuration for external APIs and related services to the specified <see cref="IServiceCollection"/>.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to add the services to.</param>
    /// <param name="configuration">The <see cref="IConfiguration"/> from which to retrieve configuration settings.</param>
    /// <returns>The <see cref="IServiceCollection"/> instance with the configured services added.</returns>
    public static IServiceCollection AddExternalApis(this IServiceCollection services, IConfiguration configuration)
        => services.Configure<ExternalApiOptions>(configuration.GetSection(ExternalApiOptions.GeneralOptionsName))
            .AddTransient<IDataAggregationService, DataAggregationService>()
            .AddWeatherApis(configuration)
            .AddNewsApis(configuration)
            .AddBooksApis(configuration);

    /// <summary>
    /// Adds configuration for weather-related APIs and related services to the specified <see cref="IServiceCollection"/>.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to add the services to.</param>
    /// <param name="configuration">The <see cref="IConfiguration"/> from which to retrieve configuration settings.</param>
    /// <returns>The <see cref="IServiceCollection"/> instance with the configured services added.</returns>
    private static IServiceCollection AddWeatherApis(this IServiceCollection services, IConfiguration configuration)
        => services.AddTransient<IDataProviderService<Weather>, DataProviderService<Weather>>()
                   .AddOpenMeteoApi(configuration);

    /// <summary>
    /// Adds configuration for the Open-Meteo API and related services to the specified <see cref="IServiceCollection"/>.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to add the services to.</param>
    /// <param name="configuration">The <see cref="IConfiguration"/> from which to retrieve configuration settings.</param>
    /// <returns>The <see cref="IServiceCollection"/> instance with the configured services added.</returns>
    private static IServiceCollection AddOpenMeteoApi(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<OpenMeteoOptions>(configuration.GetSection(OpenMeteoOptions.Name));
        services.AddTransient<IApiClient<Weather>, OpenMeteoApiClient>();
        services.AddHttpClient<OpenMeteoApiClient>()
            .ConfigureHttpClient(httpClient =>
            {
                httpClient.BaseAddress = new Uri(configuration.GetSection(OpenMeteoOptions.Name).GetValue<string>(nameof(OpenMeteoOptions.BaseAddress)));
            })
            .SetHandlerLifetime(TimeSpan.FromMinutes(configuration.GetSection(ExternalApiOptions.GeneralOptionsName).GetValue<int>(nameof(ExternalApiOptions.HandlerLifetime))));

        return services;
    }

    /// <summary>
    /// Adds configuration for news-related APIs and related services to the specified <see cref="IServiceCollection"/>.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to add the services to.</param>
    /// <param name="configuration">The <see cref="IConfiguration"/> from which to retrieve configuration settings.</param>
    /// <returns>The <see cref="IServiceCollection"/> instance with the configured services added.</returns>
    private static IServiceCollection AddNewsApis(this IServiceCollection services, IConfiguration configuration)
        => services.AddTransient<IDataProviderService<Weather>, DataProviderService<Weather>>()
                   .AddSpaceflightNewsApi(configuration);

    /// <summary>
    /// Adds configuration for the SpaceflightNews API and related services to the specified <see cref="IServiceCollection"/>.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to add the services to.</param>
    /// <param name="configuration">The <see cref="IConfiguration"/> from which to retrieve configuration settings.</param>
    /// <returns>The <see cref="IServiceCollection"/> instance with the configured services added.</returns>
    private static IServiceCollection AddSpaceflightNewsApi(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<SpaceflightNewsOptions>(configuration.GetSection(SpaceflightNewsOptions.Name));
        services.AddTransient<IApiClient<News>, SpaceflightNewsApiClient>();
        services.AddHttpClient<SpaceflightNewsApiClient>()
            .ConfigureHttpClient(httpClient =>
            {
                httpClient.BaseAddress = new Uri(configuration.GetSection(SpaceflightNewsOptions.Name).GetValue<string>(nameof(SpaceflightNewsOptions.BaseAddress)));
            })
            .SetHandlerLifetime(TimeSpan.FromMinutes(configuration.GetSection(ExternalApiOptions.GeneralOptionsName).GetValue<int>(nameof(ExternalApiOptions.HandlerLifetime))));

        return services;
    }

    /// <summary>
    /// Adds configuration for books-related APIs and related services to the specified <see cref="IServiceCollection"/>.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to add the services to.</param>
    /// <param name="configuration">The <see cref="IConfiguration"/> from which to retrieve configuration settings.</param>
    /// <returns>The <see cref="IServiceCollection"/> instance with the configured services added.</returns>
    private static IServiceCollection AddBooksApis(this IServiceCollection services, IConfiguration configuration)
        => services.AddTransient<IDataProviderService<Weather>, DataProviderService<Weather>>()
                   .AddOpenLibraryApi(configuration);

    /// <summary>
    /// Adds configuration for the Open Library API and related services to the specified <see cref="IServiceCollection"/>.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to add the services to.</param>
    /// <param name="configuration">The <see cref="IConfiguration"/> from which to retrieve configuration settings.</param>
    /// <returns>The <see cref="IServiceCollection"/> instance with the configured services added.</returns>
    private static IServiceCollection AddOpenLibraryApi(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<OpenLibraryOptions>(configuration.GetSection(OpenLibraryOptions.Name));
        services.AddTransient<IApiClient<Book>, OpenLibraryApiClient>();
        services.AddHttpClient<OpenLibraryApiClient>()
            .ConfigureHttpClient(httpClient =>
            {
                httpClient.BaseAddress = new Uri(configuration.GetSection(OpenLibraryOptions.Name).GetValue<string>(nameof(OpenLibraryOptions.BaseAddress)));
            })
            .SetHandlerLifetime(TimeSpan.FromMinutes(configuration.GetSection(ExternalApiOptions.GeneralOptionsName).GetValue<int>(nameof(ExternalApiOptions.HandlerLifetime))));

        return services;
    }
}