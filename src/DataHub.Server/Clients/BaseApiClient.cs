using DataHub.Core.Abstractions;
using DataHub.Core.Configuration.Options;
using DataHub.Core.Filters;
using DataHub.Core.Models;
using DataHub.Server.Extensions;
using DataHub.Server.Serialization;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;

namespace DataHub.Server.Clients;

/// <summary>
/// Base class for API clients providing common functionality such as caching and error handling.
/// </summary>
/// <typeparam name="T">The type of data returned by the API client.</typeparam>
public abstract class BaseApiClient<T> : IApiClient<T> where T : class
{
    protected readonly IDistributedCache _distributedCache;
    protected readonly int _cacheExpiration;

    /// <summary>
    /// Initializes a new instance of the <see cref="BaseApiClient{T}"/> class with the specified dependencies.
    /// </summary>
    /// <param name="distributedCache">The distributed cache for caching API responses.</param>
    /// <param name="options">The options for configuring the API client.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="distributedCache"/> or <paramref name="options"/> is null.</exception>
    public BaseApiClient(IDistributedCache distributedCache, IOptions<ExternalApiOptions> options)
    {
        _distributedCache = distributedCache ?? throw new ArgumentNullException(nameof(distributedCache));
        _cacheExpiration = options?.Value?.CacheExpirationInMin ?? throw new ArgumentNullException(nameof(options));
    }

    /// <summary>
    /// Retrieves data from the API using the specified date range filter.
    /// </summary>
    /// <param name="dateRange">The date range filter specifying the start and end dates for the data retrieval.</param>
    /// <returns>An <see cref="ApiClientData{T}"/> object containing the data retrieved from the API.</returns>
    public abstract Task<ApiClientData<T>> GetData(DateRangeFilter dateRange);

    /// <summary>
    /// Retrieves the API response either from the cache or by making an HTTP request.
    /// </summary>
    /// <param name="cacheKey">The cache key for storing and retrieving the API response.</param>
    /// <param name="httpTask">The function representing the HTTP request task.</param>
    /// <returns>An <see cref="HttpResponseMessage"/> representing the API response.</returns>
    protected async Task<HttpResponseMessage> GetApiResponse(string cacheKey, Func<Task<HttpResponseMessage>> httpTask)
    {
        return await _distributedCache.TryGetAndSetAsync(
            cacheKey,
            getSourceAsync: httpTask,
            jsonSerializerOptions: JsonSerializerOptionDefaults.GetDefaultSettings(),
            options: new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(_cacheExpiration)
            });
    }
}