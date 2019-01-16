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


    public class Page<T>
    {
        /// <summary>
        /// source count
        /// </summary>
        public int TotalCount { get; private set; }

        /// <summary>
        /// source count of pages
        /// </summary>
        public int PageCount { get; private set; }


        /// <summary>
        /// current page number of pages
        /// </summary>
        public int CurrentPage { get; private set; }

        /// <summary>
        /// limits records' size per page
        /// </summary>
        public int Size { get; private set; }

        /// <summary>
        /// records of current page
        /// </summary>
        public IEnumerable<T> Records { get; private set; }


        #region navigation
        /// <summary>
        /// page navigation size
        /// </summary>
        public int PageSize { get; private set; }

        /// <summary>
        /// indicate that has previous pages of the navigation
        /// 
        /// ex :   <<  6,7,8,9,10 >>  -> there are << 1,2,3,4,5 >>
        /// </summary>
        public bool HasPreviosPages { get; private set; }

        /// <summary>
        /// indicate that has more page numbers in the next of the navigation
        /// ex :   <<  6,7,8,9,10 >>  -> there are << 11,21,13,14,15 >> or more page
        /// </summary>
        public bool HasNextPages { get; private set; }


        /// <summary>
        /// 
        /// ex: current navigation <<  6,7,8,9,10 >>,  -> will be [1]
        /// </summary>
        public int PreviosLastPageNo { get; private set; }

        /// <summary>
        /// 
        /// ex: current navigation <<  6,7,8,9,10 >>,  -> will be [11] (if has more pages)
        /// </summary>
        public int NextStartPageNo { get; private set; }


        public IEnumerable<int> NavigationPages { get; private set; }

        #endregion



        public Page(
            IEnumerable<T> records,
            int totalCount,
            int pageCount,
            int currentPage,
            int size,
            int pageSize
            )
        {
            this.TotalCount = totalCount;
            this.PageCount = pageCount;
            this.CurrentPage = currentPage;
            this.Size = size;
            this.PageSize = pageSize;
            this.Records = records;

            //
            this.calculatenavigation(totalCount, pageCount, currentPage, pageSize);

        }


        private void calculatenavigation(int totalCount, int pageCount, int currentPage, int pageSize)
        {
            var isTimesOfDisplayPagesCount = (currentPage % pageSize) == 0;

            var startIndex = isTimesOfDisplayPagesCount ? ((currentPage / pageSize) - 1) * pageSize + 1
                : (currentPage / pageSize) * pageSize + 1;

            var navigationPages = Enumerable
                        .Range(startIndex, pageSize)
                        .Where(p => p <= pageCount)
                        .AsEnumerable();

            var minPage = navigationPages.Count() > 0 ? navigationPages.First() : 1;
            var maxPage = navigationPages.Count() > 0 ? navigationPages.Last() : 1;

            // binding
            this.NavigationPages = navigationPages;
            this.PreviosLastPageNo = Math.Max(minPage - 1, 1);
            this.NextStartPageNo = Math.Min(maxPage + 1, pageCount);
            this.HasPreviosPages = minPage > pageSize;
            this.HasNextPages = pageCount > maxPage;

        }

    }


}
