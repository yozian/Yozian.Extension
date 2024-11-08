using System;
using System.Collections.Generic;
using System.Linq;

namespace Yozian.Extension.Pagination;

/// <summary>
/// 
/// </summary>
/// <typeparam name="T"></typeparam>
public class Paging<T>
{
    /// <summary>
    /// Total Records Count
    /// </summary>
    public int TotalCount { get; internal set; }

    /// <summary>
    /// Total Page Count
    /// </summary>
    public int PageCount { get; internal set; }

    /// <summary>
    /// 
    /// </summary>
    public int CurrentPage { get; internal set; }

    /// <summary>
    /// Records' Size Per Page
    /// </summary>
    public int Size { get; internal set; }

    public IEnumerable<T> Records { get; internal set; }

    internal Paging(
        IQueryable<T> source,
        int? page,
        int? size
    )
    {
        this.CurrentPage = page ?? 1;
        this.Size = size ?? 10;

        this.FetchPage(source, this.CurrentPage, this.Size);
    }

    private Paging(int totalCount, int pageCount, int currentPage, int size, IEnumerable<T> records)
    {
        this.TotalCount = totalCount;
        this.PageCount = pageCount;
        this.CurrentPage = currentPage;
        this.Size = size;
        this.Records = records;
    }


    protected void FetchPage(IQueryable<T> source, int page, int size)
    {
        this.TotalCount = source.Count();
        this.PageCount = CalculatePageCount(size, this.TotalCount);
        this.CurrentPage = page >= this.PageCount ? this.PageCount : page;
        this.CurrentPage = this.CurrentPage < 1 ? 1 : this.CurrentPage;
        this.Size = size;

        this.Records = source
            .Skip((this.CurrentPage - 1) * this.Size)
            .Take(this.Size)
            .ToList();
    }

    public Paging<TOut> MapTo<TOut>(Func<T, TOut> converter)
    {
        var data = this.Records.Select(converter).ToList();

        return new Paging<TOut>(
            this.TotalCount,
            this.PageCount,
            this.CurrentPage,
            this.Size,
            data
        );
    }

    public Page<T> ToPage(int navigatorPageSize)
    {
        if (navigatorPageSize <= 0)
        {
            throw new ArgumentException("navigatorPageSize should be greater than 0!");
        }

        return new Page<T>(
            this.Records,
            this.TotalCount,
            this.PageCount,
            this.CurrentPage,
            this.Size,
            navigatorPageSize
        );
    }

    private static int CalculatePageCount(int limits, int totalCount)
    {
        if (limits == 0)
        {
            return 0;
        }

        var remainder = totalCount % limits;

        return totalCount / limits + (remainder.Equals(0) ? 0 : 1);
    }
}
