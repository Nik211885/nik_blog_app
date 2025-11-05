using Application.Adapters;
using Microsoft.Extensions.Caching.Memory;

namespace Infrastructure.Adapters;

public class CacheMemory : ICache
{
    private readonly IMemoryCache _memoryCache;
    public CacheMemory(IMemoryCache memoryCache)
    {
        _memoryCache = memoryCache;
    }
     /// <summary>
    ///  Remove item with key
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public ValueTask RemoveItem(string key)
    {
        _memoryCache.Remove(key);
        return ValueTask.CompletedTask;
    }

    /// <summary>
    ///     Set item to cache
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    /// <param name="timeSpan"></param>
    public ValueTask SetItem(string key, string value, TimeSpan timeSpan)
    {
        _memoryCache.Set(key, value, timeSpan);
        return ValueTask.CompletedTask;
    }
    /// <summary>
    ///     Get item in cache
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    /// <returns>
    ///     Return true if get item otherwise false
    /// </returns>
    public ValueTask<bool> TryGetItem(string key, out string? value)
    {
        var result = _memoryCache.TryGetValue(key, out string? v);
        value = v;
        return ValueTask.FromResult(result);
    }
}
