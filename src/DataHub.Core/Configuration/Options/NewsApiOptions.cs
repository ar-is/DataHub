namespace DataHub.Core.Configuration.Options;

/// <summary>
/// Represents options for configuring News API clients.
/// </summary>
public class NewsApiOptions : ExternalApiOptions
{
    /// <summary>
    /// The name of the general options section within the external API options.
    /// </summary>
    protected new const string Name = ExternalApiOptions.Name + ":News";
}