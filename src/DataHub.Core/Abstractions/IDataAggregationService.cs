using DataHub.Core.Filters;
using DataHub.Core.Models;

namespace AggregationDataApp.Core.Abstractions;

/// <summary>
/// Service interface for aggregating data from multiple sources.
/// </summary>
public interface IDataAggregationService
{
    /// <summary>
    /// Retrieves aggregated data based on the specified category and date range filter.
    /// </summary>
    /// <param name="category">The category of data to retrieve.</param>
    /// <param name="dateRange">The date range filter to apply.</param>
    /// <returns>A task representing the asynchronous operation. The task result contains the aggregated data.</returns>
    Task<AggregatedData> GetAggregatedData(string category, DateRangeFilter dateRange);
}