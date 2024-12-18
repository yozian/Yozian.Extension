﻿using System;
using System.Collections.Generic;
using System.Linq;
using Yozian.Extension.Dtos;

namespace Yozian.Extension;

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
    /// the result is considered by the source, and all the null items will be ignored and filtered
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
        var source = @this.Where(x => x != null).ToList();
        var destTarget = target.Where(x => x != null);

        var equalItems = source
            .Select(
                x =>
                {
                    var y = destTarget.FirstOrDefault(j => comparer.Equals(x, j));

                    // null value should treat as different
                    if (y == null)
                    {
                        return null;
                    }

                    // fix some value type would not be null, or have the default value, so we should use the comparer to compare
                    return comparer.Equals(x, y) ? new CollectionDifference<T>.Difference(x, y) : null;
                }
            )
            .Where(x => x != null)
            .ToList();

        var sourceMissingItems = destTarget.Except(@this, comparer)
            .ToList();

        var targetMissingItems = source.Except(target, comparer)
            .ToList();

        return new CollectionDifference<T>(equalItems, sourceMissingItems, targetMissingItems);
    }


   
}
