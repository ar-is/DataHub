namespace DataHub.Core.Models;

/// <summary>
/// The aggregated data fetched from multiple API client providers.
/// </summary>
/// <typeparam name="T">The type of data fetched from the API client providers.</typeparam>
public class Data<T> where T : class
{
    /// <summary>
    /// Collection of API client data providers.
    /// </summary>
    public List<ApiClientData<T>> ApiProviders { get; set; } = [];
}