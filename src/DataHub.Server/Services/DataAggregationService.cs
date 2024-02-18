using DataHub.Core.Abstractions;
using DataHub.Core.Filters;
using DataHub.Core.Models;

namespace DataHub.Server.Services;

/// <summary>
/// Service for aggregating data from multiple sources.
/// </summary>
public class DataAggregationService : IDataAggregationService
{
    private readonly IDataProviderService<Weather> _weatherDataProvider;
    private readonly IDataProviderService<News> _newsDataProvider;
    private readonly IDataProviderService<Book> _booksDataProvider;

    /// <summary>
    /// Initializes a new instance of the <see cref="DataAggregationService"/> class with the specified data provider services.
    /// </summary>
    /// <param name="weatherApiProvider">The data provider service for weather data.</param>
    /// <param name="newsApiProvider">The data provider service for news data.</param>
    /// <param name="booksApiProvider">The data provider service for book data.</param>
    /// <exception cref="ArgumentNullException">Thrown when any of the data provider services is null.</exception>
    public DataAggregationService(
        IDataProviderService<Weather> weatherApiProvider,
        IDataProviderService<News> newsApiProvider,
        IDataProviderService<Book> booksApiProvider)
    {
        _weatherDataProvider = weatherApiProvider ?? throw new ArgumentNullException(nameof(weatherApiProvider));
        _newsDataProvider = newsApiProvider ?? throw new ArgumentNullException(nameof(newsApiProvider));
        _booksDataProvider = booksApiProvider ?? throw new ArgumentNullException(nameof(booksApiProvider));
    }

    /// <inheritdoc/>
    public async Task<AggregatedData> GetAggregatedData(string category, DateRangeFilter dateRange)
    {
        var weatherDataTask = _weatherDataProvider.GetData(category, dateRange);
        var newsDataTask = _newsDataProvider.GetData(category, dateRange);
        var booksDataTask = _booksDataProvider.GetData(category, dateRange);

        await Task.WhenAll(weatherDataTask, newsDataTask, booksDataTask);

        return new AggregatedData
        {
            Weather = weatherDataTask.Result,
            News = newsDataTask.Result,
            Books = booksDataTask.Result
        };
    }
}