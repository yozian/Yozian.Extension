using System;
using System.Collections.Generic;
using System.Linq;

namespace Yozian.Extension.Pagination
{
    public class Paging<T>
    {
        public int TotalCount { get; internal set; }

        public int PageCount { get; internal set; }

        public int CurrentPage { get; internal set; }

        /// <summary>
        /// Maximum
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

            this.fetchPage(source, this.CurrentPage, this.Size);
        }

        private Paging(int totalCount, int pageCount, int currentPage, int size, IEnumerable<T> records)
        {
            this.TotalCount = totalCount;
            this.PageCount = pageCount;
            this.CurrentPage = currentPage;
            this.Size = size;
            this.Records = records;
        }


        protected void fetchPage(IQueryable<T> source, int page, int size)
        {
            this.TotalCount = source.Count();
            this.PageCount = calculatePageCount(size, this.TotalCount);
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

            return new Paging<TOut>(this.TotalCount, this.PageCount, this.CurrentPage, this.Size, data);
        }

        public Page<T> ToPage(int navigatorPageSize)
        {
            if (1 > navigatorPageSize)
            {
                throw new ArgumentException("navigatorPageSize should be greater than 0");
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

        private static int calculatePageCount(int limits, int totalCount)
        {
            if (limits == 0)
            {
                return 0;
            }

            var remainder = totalCount % limits;

            return totalCount / limits + (remainder.Equals(0) ? 0 : 1);
        }
    }
}
