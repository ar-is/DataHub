namespace DataHub.Core.Models;

/// <summary>
/// News article data.
/// </summary>
public class News
{
    /// <summary>
    /// The title of the news article.
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    /// The URL of the news article.
    /// </summary>
    public string Url { get; set; }

    /// <summary>
    /// The site or source of the news article.
    /// </summary>
    public string Site { get; set; }

    /// <summary>
    /// The summary or brief description of the news article.
    /// </summary>
    public string Summary { get; set; }

    /// <summary>
    /// The date when the news article was published.
    /// </summary>
    public DateTimeOffset DatePublished { get; set; }
}