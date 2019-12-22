using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yozian.Extension.Pagination.Models;

namespace Yozian.Extension.Pagination
{
    public static class PaginationExtension
    {
        public static Pageable<TSource, TOutput> ToPagination<TSource, TOutput>(
            this IQueryable<TSource> @this,
            int? page,
            int? size,
            Func<TSource, TOutput> converter = null
            )
        {
            return new Pageable<TSource, TOutput>(@this, page, size, converter);
        }

        public static Pageable<T> ToPagination<T>(
           this IQueryable<T> @this,
           int? page,
           int? size
           )
        {
            return new Pageable<T>(@this, page, size, t => t);
        }

        public static Task<Pageable<TSource, TOutput>> ToPaginationAsync<TSource, TOutput>(
                   this IQueryable<TSource> @this,
                   int? page,
                   int? size,
                   Func<TSource, TOutput> converter = null
                   )
        {
            return Task.Run(() => ToPagination(@this, page, size, converter));
        }

        public static Task<Pageable<T>> ToPaginationAsync<T>(
           this IQueryable<T> @this,
           int? page,
           int? size
           )
        {
            return Task.Run(() => ToPagination(@this, page, size));
        }
    }
}
