using System;
using System.Linq;
using System.Linq.Expressions;

namespace Yozian.Extension;

public static class IQueryableExtension
{
    extension<T>(IQueryable<T> @this)
    {
        public IQueryable<T> WhereWhen(
            Func<bool> condition,
            Expression<Func<T, bool>> predicate
        )
        {
            return condition() ? @this.Where(predicate) : @this;
        }

        public IQueryable<T> WhereWhen(
            bool condition,
            Expression<Func<T, bool>> predicate
        )
        {
            return condition ? @this.Where(predicate) : @this;
        }
    }
}