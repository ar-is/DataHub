namespace DataHub.Core.Models;

/// <summary>
/// The data fetched from an API client provider.
/// </summary>
/// <typeparam name="T">The type of data fetched from the API client provider.</typeparam>
public class ApiClientData<T> where T : class
{
    /// <summary>
    /// The name of the API client provider.
    /// </summary>
    public string ApiProvider { get; set; }

    /// <summary>
    /// The data collection fetched from the API client provider.
    /// </summary>
    public IEnumerable<T> Data { get; set; } = [];

    /// <summary>
    /// Value indicating whether the data fetching operation was successful.
    /// </summary>
    public bool IsSuccessful { get; set; }

    /// <summary>
    /// The error message if the data fetching operation failed.
    /// </summary>
    public string ErrorMessage { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="ApiClientData{T}"/> class with the specified API provider.
    /// </summary>
    /// <param name="apiProvider">The name of the API client provider.</param>
    private ApiClientData(string apiProvider)
    {
        ApiProvider = apiProvider;
    }

    public static ApiClientData<T> Success(string apiProvider, IEnumerable<T> data)
        => new(apiProvider)
        {
            IsSuccessful = true,
            Data = data
        };

    public static ApiClientData<T> Fail(string apiProvider)
        => new(apiProvider)
        {
            IsSuccessful = false,
            ErrorMessage = $"Could not fetch data successfully from {apiProvider}."
        };
}