using System.Text.Json;
using System.Text.Json.Serialization;

namespace DataHub.Server.Serialization;

/// <summary>
/// Converts JSON to and from <see cref="HttpContent"/>.
/// </summary>
public class HttpContentConverter : JsonConverter<HttpContent>
{
    /// <summary>
    /// Reads JSON and converts it to <see cref="HttpContent"/>.
    /// </summary>
    /// <param name="reader">The reader.</param>
    /// <param name="typeToConvert">The type to convert.</param>
    /// <param name="options">The serializer options.</param>
    /// <returns>An instance of <see cref="HttpContent"/>.</returns>
    public override HttpContent Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return new StringContent(reader.GetString());
    }

    /// <summary>
    /// Writes the specified <see cref="HttpContent"/> to JSON.
    /// </summary>
    /// <param name="writer">The JSON writer.</param>
    /// <param name="value">The value to convert.</param>
    /// <param name="options">The serializer options.</param>
    public override void Write(Utf8JsonWriter writer, HttpContent value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ReadAsStringAsync().Result);
    }
}