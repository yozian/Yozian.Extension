﻿using System;
using System.Linq;
using System.Linq.Expressions;

namespace Yozian.Extension;

public static class IQueryableExtension
{
    public static IQueryable<T> WhereWhen<T>(
        this IQueryable<T> query,
        Func<bool> condition,
        Expression<Func<T, bool>> predicate
    )
    {
        return condition() ? query.Where(predicate) : query;
    }

    public static IQueryable<T> WhereWhen<T>(
        this IQueryable<T> query,
        bool condition,
        Expression<Func<T, bool>> predicate
    )
    {
        return condition ? query.Where(predicate) : query;
    }
}
