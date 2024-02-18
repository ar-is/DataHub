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
/// Provides methods for accessing news data from the SpaceflightNews API.
/// </summary>
public class SpaceflightNewsApiClient : IApiClient<News>
{
    private readonly HttpClient _httpClient;
    private readonly IDistributedCache _distributedCache;
    private readonly string _clientName;

    /// <summary>
    /// Initializes a new instance of the <see cref="SpaceflightNewsApiClient"/> class with the specified dependencies.
    /// </summary>
    /// <param name="httpClientFactory">The factory for creating <see cref="HttpClient"/> instances.</param>
    /// <param name="distributedCache">The distributed cache for caching API responses.</param>
    /// <param name="options">The options for configuring the SpaceflightNews API client.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="httpClientFactory"/>, <paramref name="distributedCache"/>, or <paramref name="options"/> is null.</exception>
    public SpaceflightNewsApiClient(IHttpClientFactory httpClientFactory, IDistributedCache distributedCache, IOptions<SpaceflightNewsOptions> options)
    {
        _httpClient = httpClientFactory?.CreateClient(nameof(SpaceflightNewsApiClient)) ?? throw new ArgumentNullException(nameof(httpClientFactory));
        _distributedCache = distributedCache ?? throw new ArgumentNullException(nameof(distributedCache));
        _clientName = options?.Value?.ClientName ?? throw new ArgumentNullException(nameof(options));
    }

    /// <summary>
    /// Retrieves weather data from the SpaceflightNews API based on the specified date range.
    /// </summary>
    /// <param name="dateRange">The date range filter specifying the start and end dates for the data retrieval.</param>
    /// <returns>An <see cref="ApiClientData{T}"/> object containing the news data retrieved from the API.</returns>
    public async Task<ApiClientData<News>> GetData(DateRangeFilter dateRange)
    {
        var response = await _distributedCache.TryGetAndSetAsync(
            cacheKey: $"{nameof(SpaceflightNewsApiClient)}.{nameof(DateRangeFilter)}={dateRange.DateFrom}.{dateRange.DateTo}",
            getSourceAsync: async () =>
                await _httpClient.GetAsync(_httpClient.BaseAddress + $"?published_at_gte={dateRange?.DateFrom?.ToString("yyyy-MM-dd")}&published_at_lte={dateRange?.DateTo?.ToString("yyyy-MM-dd")}"),
            jsonSerializerOptions: JsonSerializerOptionDefaults.GetDefaultSettings(),
            options: new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
            });

        if (!response.IsSuccessStatusCode)
        {
            return new ApiClientData<News>(_clientName)
            {
                ErrorMessage = await response.ProcessErrorResponse()
            };
        }

        var meteoResponse = JsonSerializer.Deserialize<SpaceflightNewsApiResponse>(await response.Content.ReadAsStringAsync(), JsonSerializerOptionDefaults.GetDefaultSettings());
        return meteoResponse.ToNewsData(_clientName);
    }
}