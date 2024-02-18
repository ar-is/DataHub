using DataHub.Server.Authentication;
using DataHub.Server.Endpoints;
using DataHub.Server.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace DataHub.Server.Apis;

public static class AuthenticationApi
{
    /// <summary>
    /// Maps the Data Aggregation API endpoints.
    /// </summary>
    /// <param name="builder">The endpoint route builder to map the endpoints to.</param>
    /// <returns>The same endpoint route builder for method chaining.</returns>
    public static IEndpointRouteBuilder MapAuthentication(this IEndpointRouteBuilder builder)
    {
        var group = builder.MapGroup("api")
                           .WithTags("Authentication")
                           .ProducesProblem(StatusCodes.Status401Unauthorized);

        group
            .WithOpenApi()
            .AllowAnonymous();

        // GET: /api/authenticate/
        group.MapGet("/authenticate", AuthenticationEndpoints.Authenticate)
             .WithName(nameof(AuthenticationEndpoints.Authenticate))
             .WithSummary("Authenticate user and return access token.")
             .Produces<AuthenticationResult>(StatusCodes.Status200OK);

        return builder;
    }
}