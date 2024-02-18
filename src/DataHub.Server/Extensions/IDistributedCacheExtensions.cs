using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;

namespace DataHub.Server.Extensions;

/// <summary>
/// Provides extension methods for instances of <see cref="IDistributedCache"/>.
/// </summary>
public static class IDistributedCacheExtensions
{
    /// <summary>
    /// Tries to retrieve an item from the cache by using a unique key. <br/> 
    /// If the item is not found, the provided source is used and the item is then saved in the cache.
    /// </summary>
    /// <typeparam name="T">The type of item expected from the cache.</typeparam>
    /// <param name="cache">The instance of distributed cache service.</param>
    /// <param name="cacheKey">The cache key to search for.</param>
    /// <param name="getSourceAsync">The delegate to use in order to retrieve the item if not found in cache.</param>
    /// <param name="options">The cache options to use when adding items to the cache.</param>
    /// <param name="jsonSerializerOptions">Provides options to be used with <see cref="JsonSerializer"/>.</param>
    /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
    /// <returns>The item found in the cache under the specified key.</returns>
    public static async Task<T> TryGetAndSetAsync<T>(
        this IDistributedCache cache, 
        string cacheKey, 
        Func<Task<T>> getSourceAsync, 
        DistributedCacheEntryOptions options, 
        JsonSerializerOptions jsonSerializerOptions = null, 
        CancellationToken cancellationToken = default)
    {
        var itemJson = await cache.GetStringAsync(cacheKey, cancellationToken);
        if (!string.IsNullOrWhiteSpace(itemJson))
        {
            return JsonSerializer.Deserialize<T>(itemJson, jsonSerializerOptions);
        }
        var result = await getSourceAsync();
        if (result is null)
        {
            return await Task.FromResult(default(T));
        }
        itemJson = JsonSerializer.Serialize(result, jsonSerializerOptions);
        await cache.SetStringAsync(cacheKey, itemJson, options, cancellationToken);

        return result;
    }
}