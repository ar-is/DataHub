﻿using System.Text.Json.Serialization;
using DataHub.Core.Models;

namespace DataHub.Server.Models;

/// <summary>
/// Response from the Open-Meteo API. <br/>
/// Generated by using official open-api documentation: <see href="https://github.com/open-meteo/open-meteo/blob/main/openapi.yml"/>.
/// </summary>
public partial class OpenMeteoApiResponse
{
    /// <summary>
    /// The latitude of the weather grid-cell used to generate the forecast.
    /// </summary>
    [JsonPropertyName("latitude")]
    public double Latitude { get; set; }

    /// <summary>
    /// The longitude of the weather grid-cell used to generate the forecast.
    /// </summary>
    [JsonPropertyName("longitude")]
    public double Longitude { get; set; }

    /// <summary>
    /// The generation time of the weather forecast in milliseconds.
    /// </summary>
    [JsonPropertyName("generationtime_ms")]
    public double GenerationtimeMs { get; set; }

    /// <summary>
    /// The applied time zone offset from the specified time zone.
    /// </summary>
    [JsonPropertyName("utc_offset_seconds")]
    public double UtcOffsetSeconds { get; set; }

    /// <summary>
    /// The time zone.
    /// </summary>
    [JsonPropertyName("timezone")]
    public string Timezone { get; set; }

    /// <summary>
    /// The time zone abbreviation.
    /// </summary>
    [JsonPropertyName("timezone_abbreviation")]
    public string TimezoneAbbreviation { get; set; }

    /// <summary>
    /// The elevation in meters of the selected weather grid-cell.
    /// </summary>
    [JsonPropertyName("elevation")]
    public double Elevation { get; set; }

    /// <summary>
    /// The units for current weather conditions.
    /// </summary>
    [JsonPropertyName("current_units")]
    public CurrentUnits CurrentUnits { get; set; }

    /// <summary>
    /// The current weather conditions.
    /// </summary>
    [JsonPropertyName("current")]
    public Current Current { get; set; }

    /// <summary>
    /// The units for daily weather variables.
    /// </summary>
    [JsonPropertyName("daily_units")]
    public DailyUnits DailyUnits { get; set; }

    /// <summary>
    /// The daily weather forecast.
    /// </summary>
    [JsonPropertyName("daily")]
    public Daily Daily { get; set; }

    /// <summary>
    /// Converts the <see cref="OpenMeteoApiResponse"/> to Weather data for the Open-Meteo API client.
    /// </summary>
    /// <param name="apiProvider">The name of the API provider client.</param>
    /// <returns>An <see cref="ApiClientData{Weather}"/>instance containing weather data.</returns>
    public ApiClientData<Weather> ToWeatherData(string apiProvider)
    {
        var tempsPerDate = Daily?.Time.Zip(Daily?.Temperature2MMax, (date, temperature) => (date, temperature));
        return new(apiProvider, isSuccessful: true)
        {
            Data = tempsPerDate.Select(x => new Weather
            {
                Latitude = Latitude,
                Longitude = Longitude,
                TemperatureUnit = DailyUnits?.Temperature2MMax,
                Date = x.date,
                Temperature = x.temperature
            })
        };
    }
}

/// <summary>
/// The current weather conditions.
/// </summary>
public partial class Current
{
    /// <summary>
    /// The time of the current weather conditions.
    /// </summary>
    [JsonPropertyName("time")]
    public string Time { get; set; }

    /// <summary>
    /// The interval of the current weather conditions.
    /// </summary>
    [JsonPropertyName("interval")]
    public long Interval { get; set; }

    /// <summary>
    /// The temperature at 2 meters above ground level.
    /// </summary>
    [JsonPropertyName("temperature_2m")]
    public double Temperature2M { get; set; }
}

/// <summary>
/// The units for current weather conditions.
/// </summary>
public partial class CurrentUnits
{
    /// <summary>
    /// The time unit.
    /// </summary>
    [JsonPropertyName("time")]
    public string Time { get; set; }

    /// <summary>
    /// The interval unit.
    /// </summary>
    [JsonPropertyName("interval")]
    public string Interval { get; set; }

    /// <summary>
    /// The temperature unit at 2 meters above ground level.
    /// </summary>
    [JsonPropertyName("temperature_2m")]
    public string Temperature2M { get; set; }
}

/// <summary>
/// The daily weather forecast.
/// </summary>
public partial class Daily
{
    /// <summary>
    /// The time array containing ISO8601 timestamps.
    /// </summary>
    [JsonPropertyName("time")]
    public List<DateTimeOffset> Time { get; set; }

    /// <summary>
    /// The maximum temperature at 2 meters above ground level.
    /// </summary>
    [JsonPropertyName("temperature_2m_max")]
    public List<double> Temperature2MMax { get; set; }
}

/// <summary>
/// The units for daily weather variables.
/// </summary>
public partial class DailyUnits
{
    /// <summary>
    /// The time unit.
    /// </summary>
    [JsonPropertyName("time")]
    public string Time { get; set; }

    /// <summary>
    /// The temperature unit for the maximum temperature at 2 meters above ground level.
    /// </summary>
    [JsonPropertyName("temperature_2m_max")]
    public string Temperature2MMax { get; set; }
}