namespace DataHub.Core.Filters;

/// <summary>
/// Filters used for querying data.
/// </summary>
public class DefaultFilters
{
    /// <summary>
    /// The category filter.
    /// </summary>
    public string Category { get; set; }

    /// <summary>
    /// The start date filter.
    /// </summary>
    public string DateFrom { get; set; }

    /// <summary>
    /// The end date filter.
    /// </summary>
    public string DateTo { get; set; }
}