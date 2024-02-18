using DataHub.Core.Abstractions;
using DataHub.Core.Filters;
using DataHub.Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace DataHub.Server.Endpoints;

/// <summary>
/// Provides endpoints for retrieving aggregated data.
/// </summary>
public static class AggregatedDataEndpoints
{
    /// <summary>
    /// Retrieves aggregated data based on the provided filters.
    /// </summary>
    /// <param name="filters">The filters to apply.</param>
    /// <param name="service">The data aggregation service.</param>
    /// <returns>
    /// A task that represents the asynchronous operation.
    /// The task result contains the aggregated data if found with <see cref="HttpStatusCode.OK"/>, or a not found result with <see cref="HttpStatusCode.NotFound"/>.
    /// </returns>
    internal static async Task<Results<Ok<AggregatedData>, UnauthorizedHttpResult, NotFound>> GetAggregatedData(
        [AsParameters] DefaultFilters filters,
        [FromServices] IDataAggregationService service)
    {
        var result = await service.GetAggregatedData(filters.Category, new DateRangeFilter(filters.DateFrom, filters.DateTo));
        return result is null ? TypedResults.NotFound() : TypedResults.Ok(result);
    }
}