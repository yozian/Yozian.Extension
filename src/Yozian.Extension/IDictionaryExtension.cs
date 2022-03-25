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
            if (@this.ContainsKey(key))
            {
                return @this[key];
            }

            return default(TValue);
        }
    }
}
