using System.Net;
using System.Net.Http.Json;

namespace DataHub.Server.Tests.Unit.Clients;

/// <summary>
/// Fake HTTP message handler for simulating HTTP responses in unit tests.
/// </summary>
public class FakeHttpMessageHandler : HttpMessageHandler
{
    private readonly HttpStatusCode _statusCode;
    private readonly object _responseContent;

    /// <summary>
    /// Initializes a new instance of the <see cref="FakeHttpMessageHandler"/> class with the specified status code and response content.
    /// </summary>
    /// <param name="statusCode">The HTTP status code to be returned.</param>
    /// <param name="responseContent">The content of the HTTP response.</param>
    public FakeHttpMessageHandler(HttpStatusCode statusCode, object responseContent = null)
    {
        _statusCode = statusCode;
        _responseContent = responseContent;
    }

    /// <summary>
    /// Simulates sending an HTTP request by returning a fake HTTP response with the specified status code and content.
    /// </summary>
    /// <param name="request">The HTTP request message.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task representing the asynchronous operation. The task result contains the HTTP response message.</returns>
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        return await Task.FromResult(new HttpResponseMessage
        {
            StatusCode = _statusCode,
            Content = JsonContent.Create(_responseContent)
        });
    }
}