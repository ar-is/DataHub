using System.Text.Json;
using System.Text.Json.Serialization;

namespace DataHub.Server.Serialization;

/// <summary>
/// Default JSON serializer settings for the most common scenarios.
/// </summary>
public static class JsonSerializerOptionDefaults
{
    /// <summary>
    /// JSON options defaults.
    /// </summary>
    public static JsonSerializerOptions GetDefaultSettings()
    {
        var options = new JsonSerializerOptions
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            PropertyNameCaseInsensitive = true,
            WriteIndented = false,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
        options.Converters.Add(new JsonStringEnumConverter());
        options.Converters.Add(new HttpContentConverter());
        return options;
    }
}