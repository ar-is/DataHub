using DataHub.Core.Models;

namespace DataHub.Core.Abstractions;

/// <summary>
/// API response containing data of type <typeparamref name="T"/>.
/// </summary>
/// <typeparam name="T">The type of data contained in the API response.</typeparam>
public interface IApiResponse<T> where T : class
{
    /// <summary>
    /// Converts the API response to an <see cref="ApiClientData{T}"/> object.
    /// </summary>
    /// <param name="apiProvider">The name of the API provider.</param>
    /// <returns>An <see cref="ApiClientData{T}"/> object containing the data from the API response.</returns>
    ApiClientData<T> ToData(string apiProvider);
}