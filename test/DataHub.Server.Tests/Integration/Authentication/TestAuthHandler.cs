using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace DataHub.Server.Tests.Integration.Authentication;

/// <summary>
/// Authentication handler for the Test authentication scheme.
/// </summary>
internal class TestAuthHandler : AuthenticationHandler<TestAuthenticationSchemeOptions>
{
    internal const string AuthenticationScheme = "Test";

    /// <summary>
    /// Initializes a new instance of the <see cref="TestAuthHandler"/> class.
    /// </summary>
    /// <param name="options">The monitor for the Test authentication scheme options.</param>
    /// <param name="logger">The logger factory.</param>
    /// <param name="encoder">The URL encoder.</param>
    public TestAuthHandler(
        IOptionsMonitor<TestAuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder) : base(options, logger, encoder) { }

    /// <summary>
    /// Attempts to authenticate the user based on the provided username and password.
    /// </summary>
    /// <returns>An <see cref="AuthenticateResult"/> representing the outcome of the authentication attempt.</returns>
    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        var username = Options.Username;
        var password = Options.Password;

        if (username != "valid_username" || password != "valid_password")
        {
            return Task.FromResult(AuthenticateResult.Fail("Invalid username or password."));
        }

        // Authentication successful, create claims
        var claims = new[] { new Claim(ClaimTypes.Name, username) };
        var identity = new ClaimsIdentity(claims, Scheme.Name);
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, Scheme.Name);

        return Task.FromResult(AuthenticateResult.Success(ticket));
    }
}