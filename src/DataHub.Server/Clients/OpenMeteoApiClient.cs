using System.Text.Json;
using DataHub.Core.Abstractions;
using DataHub.Core.Filters;
using DataHub.Core.Models;
using DataHub.Server.Configuration.Options;
using DataHub.Server.Extensions;
using DataHub.Server.Models;
using DataHub.Server.Serialization;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;

namespace DataHub.Server.Clients;

/// <summary>
/// Provides methods for accessing weather data from the Open-Meteo API.
/// </summary>
public class OpenMeteoApiClient : IApiClient<Weather>
{
    private readonly HttpClient _httpClient;
    private readonly IDistributedCache _distributedCache;
    private readonly string _clientName;

    /// <summary>
    /// Initializes a new instance of the <see cref="OpenMeteoApiClient"/> class with the specified dependencies.
    /// </summary>
    /// <param name="httpClientFactory">The factory for creating <see cref="HttpClient"/> instances.</param>
    /// <param name="distributedCache">The distributed cache for caching API responses.</param>
    /// <param name="options">The options for configuring the Open-Meteo API client.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="httpClientFactory"/>, <paramref name="distributedCache"/>, or <paramref name="options"/> is null.</exception>
    public OpenMeteoApiClient(IHttpClientFactory httpClientFactory, IDistributedCache distributedCache, IOptions<OpenMeteoOptions> options)
    {
        _httpClient = httpClientFactory?.CreateClient(nameof(OpenMeteoApiClient)) ?? throw new ArgumentNullException(nameof(httpClientFactory));
        _distributedCache = distributedCache ?? throw new ArgumentNullException(nameof(distributedCache));
        _clientName = options?.Value?.ClientName ?? throw new ArgumentNullException(nameof(options));
    }

    /// <summary>
    /// Retrieves weather data from the Open-Meteo API based on the specified date range.
    /// </summary>
    /// <param name="dateRange">The date range filter specifying the start and end dates for the data retrieval.</param>
    /// <returns>An <see cref="ApiClientData{T}"/> object containing the weather data retrieved from the API.</returns>
    public async Task<ApiClientData<Weather>> GetData(DateRangeFilter dateRange)
    {
        var response = await _distributedCache.TryGetAndSetAsync(
            cacheKey: $"{nameof(OpenMeteoApiClient)}.{nameof(DateRangeFilter)}={dateRange.DateFrom}.{dateRange.DateTo}",
            getSourceAsync: async () =>
                await _httpClient.GetAsync(
                    _httpClient.BaseAddress + $"&start_date={dateRange?.DateFrom?.ToString("yyyy-MM-dd")}&end_date={dateRange?.DateTo?.ToString("yyyy-MM-dd")}"),
            jsonSerializerOptions: JsonSerializerOptionDefaults.GetDefaultSettings(),
            options: new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
            });

        if (!response.IsSuccessStatusCode)
        {
            return new ApiClientData<Weather>(_clientName)
            {
                ErrorMessage = await response.ProcessErrorResponse()
            };
        }

        var meteoResponse = JsonSerializer.Deserialize<OpenMeteoApiResponse>(await response.Content.ReadAsStringAsync(), JsonSerializerOptionDefaults.GetDefaultSettings());
        return meteoResponse.ToWeatherData(_clientName);
    }
}