using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;

namespace DataHub.Server.Tests.Integration.Authentication;

/// <summary>
/// Factory for creating test instances of the web application.
/// </summary>
/// <typeparam name="TProgram">The type of the program class.</typeparam>
internal class TestWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
{
    private readonly Action<TestAuthenticationSchemeOptions> _authOptions;

    /// <summary>
    /// Initializes a new instance of the <see cref="TestWebApplicationFactory{TProgram}"/> class.
    /// </summary>
    /// <param name="authOptions">The authentication options action.</param>
    public TestWebApplicationFactory(Action<TestAuthenticationSchemeOptions> authOptions)
    {
        _authOptions = authOptions ?? throw new ArgumentNullException(nameof(authOptions));
    }

    /// <inheritdoc/>
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            // Register Test Auth Scheme & Handler
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = TestAuthHandler.AuthenticationScheme;
                options.DefaultChallengeScheme = TestAuthHandler.AuthenticationScheme;
            })
            .AddScheme<TestAuthenticationSchemeOptions, TestAuthHandler>(TestAuthHandler.AuthenticationScheme, _authOptions);

            // Register custom policy evaluator to always authenticate API requests using Test scheme
            services.AddTransient<IPolicyEvaluator>(serviceProvider => new TestPolicyEvaluator(
                ActivatorUtilities.CreateInstance<PolicyEvaluator>(serviceProvider),
                TestAuthHandler.AuthenticationScheme));
        });

        base.ConfigureWebHost(builder);
    }
}