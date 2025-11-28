using System.Collections.Generic;
using System.Globalization;
using StringComparer = System.StringComparer;

namespace Yozian.Extension;

public static class IDictionaryExtension
{
    extension<TKey, TValue>(IDictionary<TKey, TValue> @this)
    {
        public TValue SafeGet(
            TKey key
        )
        {
            if (@this.TryGetValue(key, out var value))
            {
                return value;
            }

            return default;
        }
    }

    /// <summary>
    /// CAUTION: this method will reduce the performance for lots of keys, because it will iterate all the keys, not by the key
    /// </summary>
    extension<TValue>(IDictionary<string, TValue> @this)
    {
        public TValue SafeGet(
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

            return default;
        }
    }

    extension<TKey, TValue>(Dictionary<TKey, TValue> @this)
    {
        public void MergeDictionary(
            Dictionary<TKey, TValue> others
        )
        {
            others
                .ForEach(kv => { @this.TryAdd(kv.Key, kv.Value); }
                );
        }
    }
}