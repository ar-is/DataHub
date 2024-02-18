namespace DataHub.Core.Configuration.Options;

/// <summary>
/// Represents options for configuring external API clients.
/// </summary>
public abstract class ExternalApiOptions
{
    /// <summary>
    /// The name of the external API options section.
    /// </summary>
    public const string Name = "ExtenalApis";

    /// <summary>
    /// The name of the general options section within the external API options.
    /// </summary>
    public const string GeneralOptionsName = Name + ":General";

    /// <summary>
    /// The name of the API client.
    /// </summary>
    public string ClientName { get; set; }

    /// <summary>
    /// The base address of the API.
    /// </summary>
    public string BaseAddress { get; set; }

    /// <summary>
    /// The lifetime of the HTTP message handler used by the API client.
    /// </summary>
    public string HandlerLifetime { get; set; }

    /// <summary>
    /// The number of retry attempts for failed requests.
    /// </summary>
    public string RetryCount { get; set; }
}