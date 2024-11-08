using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Yozian.Extension.Pagination;

public static class PaginationExtension
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="this"></param>
    /// <param name="page"></param>
    /// <param name="size"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static Paging<T> ToPagination<T>(
        this IQueryable<T> @this,
        int? page,
        int? size
    )
    {
        return new Paging<T>(@this, page, size);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="this"></param>
    /// <param name="page"></param>
    /// <param name="size"></param>
    /// <param name="converter"></param>
    /// <typeparam name="TSource"></typeparam>
    /// <typeparam name="TOutput"></typeparam>
    /// <returns></returns>
    public static Paging<TOutput> ToPagination<TSource, TOutput>(
        this IQueryable<TSource> @this,
        int? page,
        int? size,
        Func<TSource, TOutput> converter
    )
    {
        return new Paging<TSource>(@this, page, size).MapTo(converter);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="this"></param>
    /// <param name="page"></param>
    /// <param name="size"></param>
    /// <param name="converter"></param>
    /// <param name="cancellationToken"></param>
    /// <typeparam name="TSource"></typeparam>
    /// <typeparam name="TOutput"></typeparam>
    /// <returns></returns>
    public static Task<Paging<TOutput>> ToPaginationAsync<TSource, TOutput>(
        this IQueryable<TSource> @this,
        int? page,
        int? size,
        Func<TSource, TOutput> converter = null,
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
    /// <param name="this"></param>
    /// <param name="page"></param>
    /// <param name="size"></param>
    /// <param name="cancellationToken"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static Task<Paging<T>> ToPaginationAsync<T>(
        this IQueryable<T> @this,
        int? page,
        int? size,
        CancellationToken cancellationToken = default
    )
    {
        return Task.Run(() => ToPagination(@this, page, size), cancellationToken);
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
