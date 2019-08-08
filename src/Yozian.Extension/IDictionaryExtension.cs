using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yozian.Extension
{
    public static class IDictionaryExtension
    {
        public static TValue SafeGet<TKey, TValue>(
            this IDictionary<TKey, TValue> @this,
            TKey key)
        {
            if (@this.ContainsKey(key))
            {
                return @this[key];
            }
            else
            {
                return default(TValue);
            }
        }
    }
}