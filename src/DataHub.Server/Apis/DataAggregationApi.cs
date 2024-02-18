using DataHub.Core.Models;
using DataHub.Server.Endpoints;
using DataHub.Server.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace DataHub.Server.Apis;

/// <summary>
/// The Data Aggregation API.
/// </summary>
public static class DataAggregationApi
{
    /// <summary>
    /// Maps the Data Aggregation API endpoints.
    /// </summary>
    /// <param name="builder">The endpoint route builder to map the endpoints to.</param>
    /// <returns>The same endpoint route builder for method chaining.</returns>
    public static IEndpointRouteBuilder MapDataAggregation(this IEndpointRouteBuilder builder)
    {
        var group = builder.MapGroup("api")
                           .WithTags("DataAggregation")
                           .ProducesProblem(StatusCodes.Status400BadRequest)
                           .ProducesProblem(StatusCodes.Status401Unauthorized)
                           .ProducesProblem(StatusCodes.Status500InternalServerError);

        group
            .RequireAuthorization();

        group
            .WithOpenApi()
            .AddOpenApiSecurityRequirement(JwtBearerDefaults.AuthenticationScheme, []);

        // GET: /api/data-aggregation/
        group.MapGet("data-aggregation", AggregatedDataEndpoints.GetAggregatedData)
             .WithName(nameof(AggregatedDataEndpoints.GetAggregatedData))
             .WithSummary("Gets Aggregated Data from various external API sources.")
             .Produces<AggregatedData>(StatusCodes.Status200OK)
             .Produces(StatusCodes.Status404NotFound);

        return builder;
    }
}