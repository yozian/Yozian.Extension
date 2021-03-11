using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Yozian.Extension
{
    public static class ICollectionExtension
    {
        public static void AddWhen<T>(
            this ICollection<T> @this,
            bool condition,
            T item
            )
        {
            if (condition)
            {
                @this.Add(item);
            }
        }

        public static void RemoveWhen<T>(
            this ICollection<T> @this,
            bool condition,
            T item
            )
        {
            if (condition)
            {
                @this.Remove(item);
            }
        }

        public static void AddRange<T>(this ICollection<T> @this, IEnumerable<T> items)
        {
            foreach (var item in items)
            {
                @this.Add(item);
            }
        }
    }
}
