using System;
using System.Collections.Generic;
using System.Linq;

namespace Yozian.Extension.Pagination
{
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
            this.CalculateNavigation(totalCount, pageCount, currentPage, pageSize);
        }

        private void CalculateNavigation(int totalCount, int pageCount, int currentPage, int pageSize)
        {
            var isTimesOfDisplayPagesCount = currentPage % pageSize == 0;

            var startIndex = isTimesOfDisplayPagesCount
                ? (currentPage / pageSize - 1) * pageSize + 1
                : currentPage / pageSize * pageSize + 1;

            var navigationPages = Enumerable
               .Range(startIndex, pageSize)
               .Where(p => p <= pageCount)
               .ToList();

            var minPage = navigationPages.Any() ? navigationPages.First() : 1;
            var maxPage = navigationPages.Any() ? navigationPages.Last() : 1;

            // binding
            this.NavigationPages = navigationPages;
            this.PreviousLastPageNo = Math.Max(minPage - 1, 1);
            this.NextStartPageNo = Math.Min(maxPage + 1, pageCount);
            this.HasPreviousPages = minPage > pageSize;
            this.HasNextPages = pageCount > maxPage;
        }

        #region navigation

        /// <summary>
        /// page navigation size
        /// </summary>
        public int PageSize { get; private set; }

        /// <summary>
        /// indicate that has previous pages of the navigation
        ///
        /// ex :   [  6,7,8,9,10 ]  -> there are [ 1,2,3,4,5 ]
        /// </summary>
        public bool HasPreviousPages { get; private set; }

        /// <summary>
        /// indicate that has more page numbers in the next of the navigation
        /// ex :   [  6,7,8,9,10 ]  -> there are [ 11,21,13,14,15 ] or more page
        /// </summary>
        public bool HasNextPages { get; private set; }

        /// <summary>
        ///
        /// ex: current navigation [  6,7,8,9,10 ],  -> will be [1]
        /// </summary>
        public int PreviousLastPageNo { get; private set; }

        /// <summary>
        ///
        /// ex: current navigation [  6,7,8,9,10 ],  -> will be [11] (if has more pages)
        /// </summary>
        public int NextStartPageNo { get; private set; }

        public IEnumerable<int> NavigationPages { get; private set; }

        #endregion navigation
    }
}
