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
/// Provides methods for accessing books data from the Open Library API.
/// </summary>
public class OpenLibraryApiClient : BaseApiClient<Book>
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<OpenLibraryApiClient> _logger;
    private readonly string _clientName;

    /// <summary>
    /// Initializes a new instance of the <see cref="OpenLibraryApiClient"/> class with the specified dependencies.
    /// </summary>
    /// <param name="httpClientFactory">The factory for creating <see cref="HttpClient"/> instances.</param>
    /// <param name="distributedCache">The distributed cache for caching API responses.</param>
    /// <param name="options">The options for configuring the Open Library API client.</param>
    /// <param name="logger">The logger for logging messages within the <see cref="OpenLibraryApiClient"/>.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="httpClientFactory"/>, <paramref name="distributedCache"/>, or <paramref name="options"/> is null.</exception>
    public OpenLibraryApiClient(
        IHttpClientFactory httpClientFactory, 
        IDistributedCache distributedCache, 
        IOptions<OpenLibraryOptions> options, 
        IOptions<ExternalApiOptions> externalApiOptions,
        ILogger<OpenLibraryApiClient> logger) : base(distributedCache, externalApiOptions)
    {
        _httpClient = httpClientFactory?.CreateClient(nameof(OpenLibraryApiClient)) ?? throw new ArgumentNullException(nameof(httpClientFactory));
        _clientName = options?.Value?.ClientName ?? throw new ArgumentNullException(nameof(options));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Retrieves books data from the Open Library API based on the specified date range.
    /// </summary>
    /// <param name="dateRange">The date range filter specifying the start and end dates for the data retrieval.</param>
    /// <returns>An <see cref="ApiClientData{T}"/> object containing the books data retrieved from the API.</returns>
    public override async Task<ApiClientData<Book>> GetData(DateRangeFilter dateRange)
    {
        var response = await GetApiResponse(
            cacheKey: $"{nameof(OpenLibraryApiClient)}.{nameof(DateRangeFilter)}={dateRange.DateFrom}.{dateRange.DateTo}",
            httpTask: async () =>
                await _httpClient.GetAsync(_httpClient.BaseAddress + $"&published_in={dateRange.DateFrom?.Year.ToString()}-{dateRange.DateTo?.Year.ToString()}"));

        if (!response.IsSuccessStatusCode)
        {
            _logger.LogWarning("Error while trying to fetch data from {ApiProvider}. {ErrorMessage}", _clientName, await response.ProcessErrorResponse());
            return ApiClientData<Book>.Fail(_clientName);
        }

        try
        {
            var meteoResponse = JsonSerializer.Deserialize<OpenLibraryApiResponse>(await response.Content.ReadAsStringAsync(), JsonSerializerOptionDefaults.GetDefaultSettings());
            return meteoResponse.ToData(_clientName);
        } catch (Exception ex)
        {
            _logger.LogWarning("Error while trying to deserialize and map data from {ApiProvider}. {ErrorMessage}", _clientName, ex.InnerException?.Message ?? ex.Message);
            return ApiClientData<Book>.Fail(_clientName);
        }
    }
}