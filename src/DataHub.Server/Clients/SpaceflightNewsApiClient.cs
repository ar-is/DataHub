using System.Text.Json;
using DataHub.Core.Configuration.Options;
using DataHub.Core.Filters;
using DataHub.Core.Models;
using DataHub.Server.Configuration.Options;
using DataHub.Server.Extensions;
using DataHub.Server.Models;
using DataHub.Server.Serialization;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace DataHub.Server.Clients;

/// <summary>
/// Provides methods for accessing news data from the SpaceflightNews API.
/// </summary>
public class SpaceflightNewsApiClient : BaseApiClient<News>
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<SpaceflightNewsApiClient> _logger;
    private readonly string _clientName;

    /// <summary>
    /// Initializes a new instance of the <see cref="SpaceflightNewsApiClient"/> class with the specified dependencies.
    /// </summary>
    /// <param name="httpClientFactory">The factory for creating <see cref="HttpClient"/> instances.</param>
    /// <param name="distributedCache">The distributed cache for caching API responses.</param>
    /// <param name="options">The options for configuring the SpaceflightNews API client.</param>
    /// <param name="logger">The logger for logging messages within the <see cref="SpaceflightNewsApiClient"/>.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="httpClientFactory"/>, <paramref name="distributedCache"/>, or <paramref name="options"/> is null.</exception>
    public SpaceflightNewsApiClient(
        IHttpClientFactory httpClientFactory, 
        IDistributedCache distributedCache, 
        IOptions<SpaceflightNewsOptions> options, 
        IOptions<ExternalApiOptions> externalApiOptions,
        ILogger<SpaceflightNewsApiClient> logger) : base(distributedCache, externalApiOptions)
    {
        _httpClient = httpClientFactory?.CreateClient(nameof(SpaceflightNewsApiClient)) ?? throw new ArgumentNullException(nameof(httpClientFactory));
        _clientName = options?.Value?.ClientName ?? throw new ArgumentNullException(nameof(options));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Retrieves news data from the SpaceflightNews API based on the specified date range.
    /// </summary>
    /// <param name="dateRange">The date range filter specifying the start and end dates for the data retrieval.</param>
    /// <returns>An <see cref="ApiClientData{T}"/> object containing the news data retrieved from the API.</returns>
    public override async Task<ApiClientData<News>> GetData(DateRangeFilter dateRange)
    {
        var response = await GetApiResponse(
            cacheKey: $"{nameof(SpaceflightNewsApiClient)}.{nameof(DateRangeFilter)}={dateRange.DateFrom}.{dateRange.DateTo}",
            httpTask: async () =>
                 await _httpClient.GetAsync(_httpClient.BaseAddress + $"?published_at_gte={dateRange?.DateFrom?.ToString("yyyy-MM-dd")}&published_at_lte={dateRange?.DateTo?.ToString("yyyy-MM-dd")}"));

        if (!response.IsSuccessStatusCode)
        {
            _logger.LogWarning("Error while trying to fetch data from {ApiProvider}. {ErrorMessage}", _clientName, await response.ProcessErrorResponse());
            return ApiClientData<News>.Fail(_clientName);
        }

        try
        {
            var meteoResponse = JsonSerializer.Deserialize<SpaceflightNewsApiResponse>(await response.Content.ReadAsStringAsync(), JsonSerializerOptionDefaults.GetDefaultSettings());
            return meteoResponse.ToData(_clientName);
        } catch (Exception ex)
        {
            _logger.LogWarning("Error while trying to deserialize and map data from {ApiProvider}. {ErrorMessage}", _clientName, ex.InnerException?.Message ?? ex.Message);
            return ApiClientData<News>.Fail(_clientName);
        }
    }
}