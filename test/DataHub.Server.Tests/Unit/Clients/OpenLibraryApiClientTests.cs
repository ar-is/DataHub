using System.Net;
using System.Text;
using System.Text.Json;
using DataHub.Core.Filters;
using DataHub.Core.Models;
using DataHub.Server.Clients;
using DataHub.Server.Configuration.Options;
using DataHub.Server.Models;
using DataHub.Server.Serialization;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NSubstitute;

namespace DataHub.Server.Tests.Unit.Clients;

internal class OpenLibraryApiClientTests
{
    private const string ClientName = "Open Library";

    private OpenLibraryApiClient _sut;
    private IDistributedCache _distributedCache;
    private IHttpClientFactory _httpClientFactory;
    private ILogger<OpenLibraryApiClient> _logger;
    private IOptions<OpenLibraryOptions> _options;
    private DateRangeFilter _dateRange;

    [SetUp]
    public void Setup()
    {
        _distributedCache = Substitute.For<IDistributedCache>();
        _logger = Substitute.For<ILogger<OpenLibraryApiClient>>();
        _options = Options.Create(new OpenLibraryOptions { ClientName = ClientName });
        _dateRange = new DateRangeFilter("2024-02-18", "2024-02-20");
    }

    [Test]
    public async Task GetData_Should_Return_HappyPath_On_SuccessfulResponse()
    {
        // Arrange
        SetHttpClient(HttpStatusCode.OK);
        _sut = new OpenLibraryApiClient(_httpClientFactory, _distributedCache, _options, _logger);
        var expectedResult = new ApiClientData<Book>(ClientName, true);

        // Act
        var result = await _sut.GetData(_dateRange);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(expectedResult, Is.EqualTo(result));
            Assert.That(result.IsSuccessful, Is.EqualTo(true));
            Assert.That(result.ErrorMessage, Is.Null);
        });
        _httpClientFactory.Received(1).CreateClient(nameof(OpenLibraryApiClient));
    }

    [Test]
    public async Task GetData_Should_Return_ErrorData_On_UnsuccessfulResponse()
    {
        // Arrange
        SetHttpClient(HttpStatusCode.ServiceUnavailable);
        _sut = new OpenLibraryApiClient(_httpClientFactory, _distributedCache, _options, _logger);
        var expectedResult = new ApiClientData<Book>(ClientName, false);

        // Act
        var result = await _sut.GetData(_dateRange);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(expectedResult, Is.EqualTo(result));
            Assert.That(result.IsSuccessful, Is.EqualTo(false));
            Assert.That(result.ErrorMessage, Is.Not.Null);
        });
        _httpClientFactory.Received(1).CreateClient(nameof(OpenLibraryApiClient));
    }

    private void SetHttpClient(HttpStatusCode statusCode)
    {
        var fakeHttpMessageHandler = new FakeHttpMessageHandler(
            statusCode,
            new StringContent(
                JsonSerializer.Serialize(new OpenLibraryApiResponse(),
                JsonSerializerOptionDefaults.GetDefaultSettings()),
                Encoding.UTF8,
                "application/json"));
        var fakeHttpClient = new HttpClient(fakeHttpMessageHandler)
        {
            BaseAddress = new Uri("https://localhost:7208")
        };
        _httpClientFactory = Substitute.For<IHttpClientFactory>();
        _httpClientFactory.CreateClient(nameof(OpenLibraryApiClient)).Returns(fakeHttpClient);
    }
}