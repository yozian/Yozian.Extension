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

        public int Page { get; internal set; }

        /// <summary>
        /// Maximum
        /// </summary>
        public int Size { get; internal set; }

        public IEnumerable<TOutput> Records { get; internal set; }

        public bool HasNextPage
        {
            get
            {
                return this.Page < PageCount;
            }
            private set { }
        }


        public Pageable<TSource, TOutput> FetchNextPage()
        {
            var nextPage = this.Page + 1;
            if (nextPage > this.PageCount)
            {
                throw new Exception($"Source Exceeds max page {this.PageCount}");
            }

            this.fetchPage(nextPage, this.Size);
            return this;
        }

        public Task<Pageable<TSource, TOutput>> FetchNextPageAsync()
        {
            return Task.Run(() => this.FetchNextPage());
        }

        protected void fetchPage(int page, int size)
        {
            this.TotalCount = source.Count();
            this.PageCount = calculatePageCount(size, this.TotalCount);
            this.Page = page >= this.PageCount ? this.PageCount : page;
            this.Page = this.Page < 1 ? 1 : this.Page;
            this.Size = size;

            if (null != this.converter)
            {
                this.Records = this.source
                    .Select(converter)
                    .Skip((this.Page - 1) * this.Size)
                    .Take(this.Size)
                    .ToList();
            }
            else
            {
                this.Records = this.source.Skip((this.Page - 1) * this.Size)
                    .Take(this.Size)
                    .Select(converter)
                    .ToList();
            }

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

            this.Page = page ?? 1;
            this.Size = size ?? 10;

            this.fetchPage(this.Page, this.Size);
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

        public new Pageable<T> FetchNextPage()
        {
            var nextPage = this.Page + 1;
            if (nextPage > this.PageCount)
            {
                throw new Exception($"Source Exceeds max page {this.PageCount}");
            }

            this.fetchPage(nextPage, this.Size);
            return this;
        }

        public new Task<Pageable<T>> FetchNextPageAsync()
        {
            return Task.Run(() => this.FetchNextPage());
        }


    }


}
