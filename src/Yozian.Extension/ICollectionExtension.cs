using System;
using System.Collections.Generic;
using System.Linq;
using Yozian.Extension.Dtos;

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


        /// <summary>
        /// the result is considered by the source side
        /// </summary>
        /// <param name="this"></param>
        /// <param name="target"></param>
        /// <param name="comparer"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static CollectionDifference<T> DifferFrom<T>(
            this ICollection<T> @this,
            ICollection<T> target,
            Func<T, T, bool> comparer
        )
        {
            return DifferFrom(@this, target, new GenericComparer<T>(comparer));
        }


        /// <summary>
        /// the result is considered by the source side
        /// </summary>
        /// <param name="this"></param>
        /// <param name="target"></param>
        /// <param name="comparer"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static CollectionDifference<T> DifferFrom<T>(
            this ICollection<T> @this,
            ICollection<T> target,
            IEqualityComparer<T> comparer
        )
        {
            var equalItems = @this
                .Select(
                    x =>
                    {
                        var y = target.FirstOrDefault(j => comparer.Equals(x, j));

                        // fix some value type would not be null, or have the default value, so we should use the comparer to compare
                        return comparer.Equals(x, y) ? new CollectionDifference<T>.Difference(x, y) : null;
                    }
                )
                .Where(x => x != null)
                .ToList();

            var sourceMissingItems = target.Except(@this, comparer)
                .ToList();

            var targetMissingItems = @this.Except(target, comparer)
                .ToList();

            return new CollectionDifference<T>(equalItems, sourceMissingItems, targetMissingItems);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="outer"></param>
        /// <param name="inner"></param>
        /// <param name="outerKeySelector"></param>
        /// <param name="innerKeySelector"></param>
        /// <param name="resultSelector"></param>
        /// <typeparam name="TOuter"></typeparam>
        /// <typeparam name="TInner"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <returns></returns>
        public static IEnumerable<TResult> LeftOuterJoin<TOuter, TInner, TKey, TResult>(
            this IEnumerable<TOuter> outer,
            IEnumerable<TInner> inner,
            Func<TOuter, TKey> outerKeySelector,
            Func<TInner, TKey> innerKeySelector,
            Func<TOuter, TInner, TResult> resultSelector
        )
        {
            return outer
                .GroupJoin(
                    inner,
                    outerKeySelector,
                    innerKeySelector,
                    (a, b) => new
                    {
                        a,
                        b
                    }
                )
                .SelectMany(x => x.b.DefaultIfEmpty(), (x, b) => resultSelector(x.a, b));
        }
    }
}
