using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Yozian.Extension.Pagination;

public static class PaginationExtension
{
    /// <param name="this"></param>
    /// <typeparam name="T"></typeparam>
    extension<T>(IQueryable<T> @this)
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="page"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public Paging<T> ToPagination(
            int? page,
            int? size
        )
        {
            return new Paging<T>(
                @this,
                page,
                size
            );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="page"></param>
        /// <param name="size"></param>
        /// <param name="converter"></param>
        /// <typeparam name="TOutput"></typeparam>
        /// <returns></returns>
        public Paging<TOutput> ToPagination<TOutput>(
            int? page,
            int? size,
            Func<T, TOutput> converter
        )
        {
            return new Paging<T>(
                @this,
                page,
                size
            ).MapTo(converter);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="page"></param>
        /// <param name="size"></param>
        /// <param name="converter"></param>
        /// <param name="cancellationToken"></param>
        /// <typeparam name="TOutput"></typeparam>
        /// <returns></returns>
        public Task<Paging<TOutput>> ToPaginationAsync<TOutput>(
            int? page,
            int? size,
            Func<T, TOutput> converter = null,
            CancellationToken cancellationToken = default
        )
        {
            return Task.Run(
                () => ToPagination(
                    @this,
                    page,
                    size,
                    converter
                ),
                cancellationToken
            );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="page"></param>
        /// <param name="size"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<Paging<T>> ToPaginationAsync(
            int? page,
            int? size,
            CancellationToken cancellationToken = default
        )
        {
            return Task.Run(
                () => ToPagination(
                    @this,
                    page,
                    size
                ),
                cancellationToken
            );
        }
    }


    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="size">paging size</param>
    /// <returns></returns>
    public static List<Page<T>> ToPagination<T>(this IEnumerable<T> @this, int size)
    {
        var list = @this.ToList();

        var pages = new List<Page<T>>();

        var totalCount = list.Count;

        var totalPages = Convert.ToInt32(Math.Ceiling(list.Count / (size * 1.0f)));

        var currentPage = 1;

        while (currentPage <= totalPages)
        {
            var page = new Page<T>(
                list.Skip((currentPage - 1) * size).Take(size).ToList(),
                totalCount,
                totalPages,
                currentPage,
                size,
                1
            );

            pages.Add(page);

            currentPage++;
        }

        return pages;
    }
}
