namespace DataHub.Core.Filters;

/// <summary>
/// Date range filter.
/// </summary>
public class DateRangeFilter
{
    /// <summary>
    /// Default filtering date.
    /// </summary>
    private static readonly DateOnly DefaultDate = DateOnly.FromDateTime(DateTime.UtcNow);

    /// <summary>
    /// The starting date of the range.
    /// </summary>
    public DateOnly? DateFrom { get; set; }

    /// <summary>
    /// The ending date of the range.
    /// </summary>
    public DateOnly? DateTo { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="DateRangeFilter"/> class by parsing date strings parameters.
    /// </summary>
    /// <param name="dateFrom">The string starting date filter.</param>
    /// <param name="dateTo">The string ending date filter.</param>
    public DateRangeFilter(string dateFrom, string dateTo)
    {
        DateFrom = ParseDateOrDefault(dateFrom);
        DateTo = ParseDateOrDefault(dateTo);
    }

    /// <summary>
    /// Parses the date string or returns the current date if parsing fails.
    /// </summary>
    /// <param name="date">The date string to parse.</param>
    /// <returns>The parsed date or the current date if parsing fails.</returns>
    private static DateOnly ParseDateOrDefault(string date)
        => DateOnly.TryParse(date, out var parsedDate) ? parsedDate : DefaultDate;
}