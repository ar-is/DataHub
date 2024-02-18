namespace DataHub.Core.Models;

/// <summary>
/// The aggregated data fetched from multiple sources.
/// </summary>
public class AggregatedData
{
    /// <summary>
    /// The aggregated weather data.
    /// </summary>
    public Data<Weather> Weather { get; set; }

    /// <summary>
    /// The aggregated news data.
    /// </summary>
    public Data<News> News { get; set; }

    /// <summary>
    /// The aggregated book data.
    /// </summary>
    public Data<Book> Books { get; set; }
}