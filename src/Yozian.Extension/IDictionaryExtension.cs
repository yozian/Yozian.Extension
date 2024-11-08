using System.Collections.Generic;
using System.Globalization;
using StringComparer = System.StringComparer;

namespace Yozian.Extension;

public static class IDictionaryExtension
{
    public static TValue SafeGet<TKey, TValue>(
        this IDictionary<TKey, TValue> @this,
        TKey key
    )
    {
        if (@this.TryGetValue(key, out var value))
        {
            return value;
        }

        return default(TValue);
    }

    /// <summary>
    /// CAUTION: this method will reduce the performance for lots of keys, because it will iterate all the keys, not by the key
    /// </summary>
    /// <param name="this"></param>
    /// <param name="key"></param>
    /// <param name="ignoreCase"></param>
    /// <typeparam name="TValue"></typeparam>
    /// <returns></returns>
    public static TValue SafeGet<TValue>(
        this IDictionary<string, TValue> @this,
        string key,
        bool ignoreCase
    )
    {
        var comparer = StringComparer.Create(CultureInfo.InvariantCulture, ignoreCase);

        foreach (var kvp in @this)
        {
            if (comparer.Equals(kvp.Key, key))
            {
                return kvp.Value;
            }
        }

        return default(TValue);
    }

    public static void MergeDictionary<TKey, TValue>(
        this Dictionary<TKey, TValue> @this,
        Dictionary<TKey, TValue> others
    )
    {
        others
            .ForEach(
                kv => { @this.TryAdd(kv.Key, kv.Value); }
            );
    }
}
