using DataHub.Core.Configuration.Options;

namespace DataHub.Server.Configuration.Options;

/// <summary>
/// Represents options for configuring Open-Meteo API client.
/// </summary>
public class OpenMeteoOptions : WeatherApiOptions
{
    /// <summary>
    /// The name of the Open-Meteo options section within the Weather API options.
    /// </summary>
    public new const string Name = WeatherApiOptions.Name + ":Open-Meteo";
}