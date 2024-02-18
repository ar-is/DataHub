using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using DataHub.Api;
using DataHub.Server.Tests.Integration.Authentication;
using Microsoft.AspNetCore.Mvc.Testing;

namespace DataHub.Server.Tests.Integration.Apis;

[Parallelizable(ParallelScope.Children)]
internal class MyHands2ApiTests
{
    [Test]
    public async Task WhenInvalidEndpoint_ReturnsNotFound()
    {
        // Arrange
        using var httpClient = CreateClient();

        // Act
        var response = await httpClient.PostAsJsonAsync("invalid-endpoint", new { });

        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
    }

    [Test]
    public async Task DataAggegationEndpoint_WhenUnauthorized_ReturnsUnauthorized()
    {
        // Arrange
        using var httpClient = CreateClient();

        // Act
        var response = await httpClient.GetAsync(string.Empty);

        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
    }

    [Test]
    public async Task DataAggegationEndpoint_WhenValidRequest_Returns_Ok()
    {
        // Arrange
        using var httpClient = CreateClient(username: "valid_username", password: "valid_password");

        // Act
        var response = await httpClient.GetAsync(string.Empty);

        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
    }

    private static HttpClient CreateClient(string username = null, string password = null)
    {
        var factory = new TestWebApplicationFactory<Program>(options =>
        {
            options.Username = username;
            options.Password = password;
        });

        var client = factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            BaseAddress = new Uri("https://localhost:7208/api/data-aggregation/"),
            AllowAutoRedirect = false
        });
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(TestAuthHandler.AuthenticationScheme);

        return client;
    }
}