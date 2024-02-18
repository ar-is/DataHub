using DataHub.Core.Configuration.Options;

namespace DataHub.Server.Configuration.Options;

/// <summary>
/// Represents options for configuring Open Library API client.
/// </summary>
public class OpenLibraryOptions : BooksApiOptions
{
    /// <summary>
    /// The name of the Open Library options section within the Books API options.
    /// </summary>
    public new const string Name = BooksApiOptions.Name + ":OpenLibrary";
}