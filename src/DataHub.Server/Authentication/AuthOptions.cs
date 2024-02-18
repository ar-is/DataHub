namespace DataHub.Server.Authentication;

/// <summary>
/// Represents options for configuring user authentication.
/// </summary>
public class AuthOptions
{
    /// <summary>
    /// The name of the Auth Options section.
    /// </summary>
    public const string Name = "Auth";

    /// <summary>
    /// The username for authentication.
    /// </summary>
    public string Username { get; set; }

    /// <summary>
    /// The password for authentication.
    /// </summary>
    public string Password { get; set; }

    /// <summary>
    /// The signing key used to sign the JWT token.
    /// </summary>
    public string SecretKey { get; set; }

    /// <summary>
    /// The expiration time of the JWT token in minutes.
    /// </summary>
    public int ExpirationMinutes { get; set; }
}