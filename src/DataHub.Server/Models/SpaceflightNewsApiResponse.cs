﻿using System.Text.Json.Serialization;
using DataHub.Core.Models;

namespace DataHub.Server.Models;

/// <summary>
/// Represents the response from the Spaceflight News API. <br/>
/// Generated by using official open-api documentation: <see href="https://api.spaceflightnewsapi.net/v4/docs/#/articles/articles_list"/>.
/// </summary>
public partial class SpaceflightNewsApiResponse
{
    /// <summary>
    /// The total number of news articles returned in the response.
    /// </summary>
    [JsonPropertyName("count")]
    public long Count { get; set; }

    /// <summary>
    /// The URI to the next page of news articles.
    /// </summary>
    [JsonPropertyName("next")]
    public Uri Next { get; set; }

    /// <summary>
    /// The URI to the previous page of news articles.
    /// </summary>
    [JsonPropertyName("previous")]
    public object Previous { get; set; }

    /// <summary>
    /// The list of news article results.
    /// </summary>
    [JsonPropertyName("results")]
    public List<Result> Results { get; set; }

    /// <summary>
    /// Converts the <see cref="SpaceflightNewsApiResponse"/> to News data for the Spaceflight News API client.
    /// </summary>
    /// <param name="apiProvider">The name of the API provider client.</param>
    /// <returns>An <see cref="ApiClientData{News}"/>instance containing news data.</returns>
    public ApiClientData<News> ToNewsData(string apiProvider)
        => new(apiProvider, isSuccessful: true)
        {
            Data = Results.Select(x => new News
            {
                Title = x.Title,
                Url = x.Url?.AbsoluteUri,
                Site = x.NewsSite,
                Summary = x.Summary,
                DatePublished = x.PublishedAt
            })
        };
}

/// <summary>
/// Represents a single news article returned by the Spaceflight News API.
/// </summary>
public partial class Result
{
    /// <summary>
    /// The unique identifier of the news article.
    /// </summary>
    [JsonPropertyName("id")]
    public long Id { get; set; }

    /// <summary>
    /// The title of the news article.
    /// </summary>
    [JsonPropertyName("title")]
    public string Title { get; set; }

    /// <summary>
    /// The URL of the news article.
    /// </summary>
    [JsonPropertyName("url")]
    public Uri Url { get; set; }

    /// <summary>
    /// The URL of the image associated with the news article.
    /// </summary>
    [JsonPropertyName("image_url")]
    public Uri ImageUrl { get; set; }

    /// <summary>
    /// The name of the news site.
    /// </summary>
    [JsonPropertyName("news_site")]
    public string NewsSite { get; set; }

    /// <summary>
    /// The summary of the news article.
    /// </summary>
    [JsonPropertyName("summary")]
    public string Summary { get; set; }

    /// <summary>
    /// The date and time when the news article was published.
    /// </summary>
    [JsonPropertyName("published_at")]
    public DateTimeOffset PublishedAt { get; set; }

    /// <summary>
    /// The date and time when the news article was last updated.
    /// </summary>
    [JsonPropertyName("updated_at")]
    public DateTimeOffset UpdatedAt { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the news article is featured.
    /// </summary>
    [JsonPropertyName("featured")]
    public bool Featured { get; set; }

    /// <summary>
    /// The list of launches associated with the news article.
    /// </summary>
    [JsonPropertyName("launches")]
    public List<Launch> Launches { get; set; }

    /// <summary>
    /// The list of events associated with the news article.
    /// </summary>
    [JsonPropertyName("events")]
    public List<Event> Events { get; set; }
}

/// <summary>
/// Represents an event associated with a news article from the Spaceflight News API.
/// </summary>
public partial class Event
{
    /// <summary>
    /// The unique identifier of the event.
    /// </summary>
    [JsonPropertyName("event_id")]
    public long EventId { get; set; }

    /// <summary>
    /// The provider of the event.
    /// </summary>
    [JsonPropertyName("provider")]
    public string Provider { get; set; }
}

/// <summary>
/// Represents a launch associated with a news article from the Spaceflight News API.
/// </summary>
public partial class Launch
{
    /// <summary>
    /// The unique identifier of the launch.
    /// </summary>
    [JsonPropertyName("launch_id")]
    public Guid LaunchId { get; set; }

    /// <summary>
    /// The provider of the launch.
    /// </summary>
    [JsonPropertyName("provider")]
    public string Provider { get; set; }
}