using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Application.Adapters;

public interface ICache
{
    /// <summary>
    ///     Set item to cache
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    /// <param name="timeSpan"></param>
    ValueTask SetItem(string key, string value, TimeSpan timeSpan);
    /// <summary>
    ///     Get item in cache
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    /// <returns>
    ///     Return true if get item otherwise false
    /// </returns>
    ValueTask<bool> TryGetItem(string key, out string? value);
    /// <summary>
    ///  Remove item with key
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    ValueTask RemoveItem(string key);
}
