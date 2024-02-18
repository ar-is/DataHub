using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Http;

namespace DataHub.Server.Tests.Integration.Authentication;

/// <summary>
/// Policy evaluator for testing purposes.
/// </summary>
internal class TestPolicyEvaluator : IPolicyEvaluator
{
    private readonly PolicyEvaluator _innerEvaluator;
    private readonly string _authenticationScheme;

    /// <summary>
    /// Initializes a new instance of the <see cref="TestPolicyEvaluator"/> class.
    /// </summary>
    /// <param name="innerEvaluator">The inner policy evaluator.</param>
    /// <param name="authenticationScheme">The authentication scheme to be used.</param>
    public TestPolicyEvaluator(PolicyEvaluator innerEvaluator, string authenticationScheme)
    {
        _innerEvaluator = innerEvaluator ?? throw new ArgumentNullException(nameof(innerEvaluator));
        _authenticationScheme = authenticationScheme ?? throw new ArgumentNullException(nameof(authenticationScheme));
    }

    /// <inheritdoc/>
    public Task<AuthenticateResult> AuthenticateAsync(AuthorizationPolicy policy, HttpContext context)
    {
        var combinedPolicy = new AuthorizationPolicyBuilder()
            .AddAuthenticationSchemes(_authenticationScheme)
            .Combine(policy)
            .Build();

        return _innerEvaluator.AuthenticateAsync(combinedPolicy, context);
    }

    /// <inheritdoc/>
    public Task<PolicyAuthorizationResult> AuthorizeAsync(AuthorizationPolicy policy, AuthenticateResult authenticationResult, HttpContext context, object resource)
        => _innerEvaluator.AuthorizeAsync(policy, authenticationResult, context, resource);
}