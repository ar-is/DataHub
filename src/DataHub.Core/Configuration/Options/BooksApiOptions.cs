namespace DataHub.Core.Configuration.Options;

/// <summary>
/// Represents options for configuring Books API clients.
/// </summary>
public class BooksApiOptions : ExternalApiOptions
{
    /// <summary>
    /// The name of the general options section within the external API options.
    /// </summary>
    protected new const string Name = ExternalApiOptions.Name + ":Books";
}