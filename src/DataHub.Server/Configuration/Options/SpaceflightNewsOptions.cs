using DataHub.Core.Configuration.Options;

namespace DataHub.Server.Configuration.Options;

public class SpaceflightNewsOptions : NewsApiOptions
{
    /// <summary>
    /// The name of the SpaceflightNews options section within the News API options.
    /// </summary>
    public new const string Name = NewsApiOptions.Name + ":SpaceflightNews";
}