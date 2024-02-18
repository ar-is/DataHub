namespace DataHub.Server.Authentication;

/// <summary>
/// Represents the result of an authentication operation, containing an access token.
/// </summary>
public record AuthenticationResult(string AccessToken);