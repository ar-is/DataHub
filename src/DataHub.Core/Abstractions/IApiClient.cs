using DataHub.Core.Filters;
using DataHub.Core.Models;

namespace DataHub.Core.Abstractions;

/// <summary>
/// Generic API client interface for fetching data of type <typeparamref name="T"/>.
/// </summary>
/// <typeparam name="T">The type of data fetched by the API client.</typeparam>
public interface IApiClient<T> where T : class
{
    /// <summary>
    /// Fetches data of type <typeparamref name="T"/> based on the provided date range filter.
    /// </summary>
    /// <param name="dateRange">The date range filter to apply.</param>
    /// <returns>A task representing the asynchronous operation. The task result contains the fetched data wrapped in an <see cref="ApiClientData{T}"/> object.</returns>
    Task<ApiClientData<T>> GetData(DateRangeFilter dateRange);
}