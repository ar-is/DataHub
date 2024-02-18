using DataHub.Core.Abstractions;
using DataHub.Core.Filters;
using DataHub.Core.Models;
using Microsoft.Extensions.Logging;

namespace DataHub.Server.Services;

/// <summary>
/// Data provider service for fetching data of type <typeparamref name="T"/> from multiple API clients.
/// </summary>
/// <typeparam name="T">The type of data provided by the service.</typeparam>
public class DataProviderService<T> : IDataProviderService<T> where T : class
{
    private readonly IEnumerable<IApiClient<T>> _apiClients;
    private readonly ILogger<DataProviderService<T>> _logger;

    /// <summary>
    /// The category of the data provided by the service.
    /// </summary>
    public string DataCategory => typeof(T).Name;

    /// <summary>
    /// Initializes a new instance of the <see cref="DataProviderService{T}"/> class with the specified API clients.
    /// </summary>
    /// <param name="apiClients">The API clients used to fetch data.</param>
    /// <exception cref="ArgumentNullException">Thrown when the <paramref name="apiClients"/> is null.</exception>
    public DataProviderService(IEnumerable<IApiClient<T>> apiClients, ILogger<DataProviderService<T>> logger)
    {
        _apiClients = apiClients ?? throw new ArgumentNullException(nameof(apiClients));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <inheritdoc/>
    public async Task<Data<T>> GetData(string category, DateRangeFilter dateRange)
    {
        var data = new Data<T>();
        if (!string.IsNullOrWhiteSpace(category) && category != DataCategory)
        {
            return data;
        }

        try
        {
            await foreach (var clientData in FetchApiClientData(dateRange))
            {
                data.ApiProviders.Add(clientData);
            }
        } catch (Exception ex)
        {
            _logger.LogError("Something went wrong while trying to fetch {DataCategory} data - Error: {Exception}", DataCategory, ex.InnerException?.Message ?? ex.Message);
            return data;
        }

        return data;
    }

    private async IAsyncEnumerable<ApiClientData<T>> FetchApiClientData(DateRangeFilter dateRange)
    {
        await foreach (var apiClient in _apiClients.ToAsyncEnumerable())
        {
            yield return await apiClient.GetData(dateRange);
        }
    }
}