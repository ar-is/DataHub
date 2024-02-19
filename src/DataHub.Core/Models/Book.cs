namespace DataHub.Core.Models;

/// <summary>
/// Book data.
/// </summary>
public class Book
{
    /// <summary>
    /// The title of the book.
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    /// The list of authors of the book.
    /// </summary>
    public List<string> Authors { get; set; }

    /// <summary>
    /// The date when the book was published.
    /// </summary>
    public DateTimeOffset DatePublished { get; set; }
}