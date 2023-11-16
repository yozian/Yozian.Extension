using System.Collections.Generic;

namespace Yozian.Extension
{
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
    }
}
