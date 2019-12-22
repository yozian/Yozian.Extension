using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Yozian.Extension.Pagination.Models
{
    public class Pageable<TSource, TOutput>
    {
        protected IQueryable<TSource> source;

        protected Func<TSource, TOutput> converter;

        public int TotalCount { get; internal set; }
        public int PageCount { get; internal set; }

        public int CurrentPage { get; internal set; }

        [Obsolete("Please use [CurrentPage] instead.")]
        public int Page
        {
            get
            {
                return this.CurrentPage;
            }
            internal set
            {
            }
        }

        /// <summary>
        /// Maximum
        /// </summary>
        public int Size { get; internal set; }

        public IEnumerable<TOutput> Records { get; internal set; }

        public bool HasNextPage
        {
            get
            {
                return this.CurrentPage < PageCount;
            }
            private set { }
        }

        public void FetchNextPage()
        {
            var nextPage = this.CurrentPage + 1;
            if (nextPage > this.PageCount)
            {
                throw new Exception($"Source Exceeds max page {this.PageCount}");
            }

            this.fetchPage(nextPage, this.Size);
        }

        public Task FetchNextPageAsync()
        {
            return Task.Run(() => this.FetchNextPage());
        }

        protected void fetchPage(int page, int size)
        {
            this.TotalCount = source.Count();
            this.PageCount = calculatePageCount(size, this.TotalCount);
            this.CurrentPage = page >= this.PageCount ? this.PageCount : page;
            this.CurrentPage = this.CurrentPage < 1 ? 1 : this.CurrentPage;
            this.Size = size;

            if (null != this.converter)
            {
                this.Records = this.source
                    .Select(converter)
                    .Skip((this.CurrentPage - 1) * this.Size)
                    .Take(this.Size)
                    .ToList();
            }
            else
            {
                this.Records = this.source.Skip((this.CurrentPage - 1) * this.Size)
                    .Take(this.Size)
                    .Select(converter)
                    .ToList();
            }
        }

        public Page<T> ToPage<T>(int pageSize, Func<TOutput, T> converter)
        {
            if (1 > pageSize)
            {
                throw new ArgumentException($"pageSize should be greater than 0");
            }

            var data = this.Records.Select(converter).AsEnumerable();

            return new Page<T>(
                    data,
                    this.TotalCount,
                    this.PageCount,
                    this.CurrentPage,
                    this.Size,
                    pageSize
                );
        }

        public Page<TOutput> ToPage(int pageSize)
        {
            if (1 > pageSize)
            {
                throw new ArgumentException($"pageSize should be greater than 0");
            }

            return new Page<TOutput>(
                    this.Records,
                    this.TotalCount,
                    this.PageCount,
                    this.CurrentPage,
                    this.Size,
                    pageSize
                );
        }

        internal Pageable(
            IQueryable<TSource> source,
            int? page,
            int? size,
            Func<TSource, TOutput> converter
            )
        {
            this.source = source;
            this.converter = converter;

            this.CurrentPage = page ?? 1;
            this.Size = size ?? 10;

            this.fetchPage(this.CurrentPage, this.Size);
        }

        private static int calculatePageCount(int limits, int totalCount)
        {
            if (limits == 0)
            {
                return 0;
            }

            var remainder = totalCount % limits;
            return (totalCount / limits) + (remainder.Equals(0) ? 0 : 1);
        }
    }

    public class Pageable<T> : Pageable<T, T>
    {
        internal Pageable(
            IQueryable<T> source,
            int? page,
            int? size,
            Func<T, T> converter
            )
            : base(source, page, size, converter)
        {
        }

        public new void FetchNextPage()
        {
            var nextPage = this.CurrentPage + 1;
            if (nextPage > this.PageCount)
            {
                throw new Exception($"Source Exceeds max page {this.PageCount}");
            }

            this.fetchPage(nextPage, this.Size);
        }

        public new Task FetchNextPageAsync()
        {
            return Task.Run(() => this.FetchNextPage());
        }
    }
}
