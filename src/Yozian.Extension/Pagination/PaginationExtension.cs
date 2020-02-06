using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yozian.Extension.Pagination
{
    public static class PaginationExtension
    {
        public static Paging<T> ToPagination<T>(
            this IQueryable<T> @this,
            int? page,
            int? size
        )
        {
            return new Paging<T>(@this, page, size);
        }

        public static Paging<TOutput> ToPagination<TSource, TOutput>(
            this IQueryable<TSource> @this,
            int? page,
            int? size,
            Func<TSource, TOutput> converter
        )
        {
            return new Paging<TSource>(@this, page, size).MapTo(converter);
        }

        public static Task<Paging<TOutput>> ToPaginationAsync<TSource, TOutput>(
            this IQueryable<TSource> @this,
            int? page,
            int? size,
            Func<TSource, TOutput> converter = null
        )
        {
            return Task.Run(() => ToPagination(@this, page, size, converter));
        }

        public static Task<Paging<T>> ToPaginationAsync<T>(
            this IQueryable<T> @this,
            int? page,
            int? size
        )
        {
            return Task.Run(() => ToPagination(@this, page, size));
        }
    }
}