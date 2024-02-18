using Microsoft.AspNetCore.Authentication;

namespace DataHub.Server.Tests.Integration.Authentication;

/// <summary>
/// Options for the Test authentication scheme.
/// </summary>
internal class TestAuthenticationSchemeOptions : AuthenticationSchemeOptions
{
    /// <summary>
    /// The test username for authentication.
    /// </summary>
    public string Username { get; set; }

    /// <summary>
    /// The test password for authentication.
    /// </summary>
    public string Password { get; set; }
}