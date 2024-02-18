using System.Net;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;

namespace DataHub.Server.Extensions;

/// <summary>
/// Provides extension methods for processing instances of <see cref="HttpResponseMessage"/>.
/// </summary>
public static class HttpResponseMessageExtensions
{
    /// <summary>
    /// Processes the error response from an <see cref="HttpResponseMessage"/>.
    /// </summary>
    /// <param name="httpResponseMessage">The HTTP response message.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the error message.</returns>
    public static async Task<string> ProcessErrorResponse(this HttpResponseMessage httpResponseMessage)
    {
        if (httpResponseMessage.IsSuccessStatusCode)
        {
            return string.Empty;
        }

        var errorMessage = $"Status Code: {httpResponseMessage.StatusCode}, Error: ";
        if (httpResponseMessage.StatusCode == HttpStatusCode.BadRequest)
        {
            var problemDetails = JsonSerializer.Deserialize<ValidationProblemDetails>(await httpResponseMessage.Content.ReadAsStringAsync());
            if (problemDetails.Errors.Any())
            {
                var message = new StringBuilder();
                foreach (var error in problemDetails.Errors)
                {
                    message.AppendLine(error.Value[0]);
                }
                return errorMessage + message.ToString().Trim();
            }

            return errorMessage + (problemDetails.Detail ?? "Bad request with no additional details.");
        }

        return errorMessage + httpResponseMessage.ReasonPhrase;
    }
}