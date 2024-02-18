using DataHub.Core.Filters;
using DataHub.Core.Models;

namespace DataHub.Core.Abstractions;

/// <summary>
/// Generic data provider service interface for aggregating data of type <typeparamref name="T"/> from multiple <see cref="IApiClient{T}"/> sources.
/// </summary>
/// <typeparam name="T">The type of data provided by the service.</typeparam>
public interface IDataProviderService<T> where T : class
{
    /// <summary>
    /// The category of the data provided by the service.
    /// </summary>
    string DataCategory { get; }

    /// <summary>
    /// Retrieves aggregated data of type <typeparamref name="T"/> based on the specified category and date range filter.
    /// </summary>
    /// <param name="category">The category of data to retrieve.</param>
    /// <param name="dateRange">The date range filter to apply.</param>
    /// <returns>A task representing the asynchronous operation. The task result contains the aggregated data wrapped in a <see cref="Data{T}"/> object.</returns>
    Task<Data<T>> GetData(string category, DateRangeFilter dateRange);
}