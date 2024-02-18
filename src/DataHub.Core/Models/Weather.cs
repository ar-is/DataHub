namespace DataHub.Core.Models;

/// <summary>
/// Weather data for a specific location.
/// </summary>
public class Weather
{
    /// <summary>
    /// The latitude of the location.
    /// </summary>
    public double Latitude { get; set; }

    /// <summary>
    /// The longitude of the location.
    /// </summary>
    public double Longitude { get; set; }

    /// <summary>
    /// The date and time when the weather data was recorded.
    /// </summary>
    public DateTimeOffset Date { get; set; }

    /// <summary>
    /// The unit of measurement for temperature (e.g., Celsius, Fahrenheit).
    /// </summary>
    public string TemperatureUnit { get; set; }

    /// <summary>
    /// The temperature at the location.
    /// </summary>
    public double Temperature { get; set; }
}