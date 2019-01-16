using System;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using Yozian.Extension.Pagination;
using Yozian.Extension.Pagination.Models;
using Yozian.Extension.Test.Data;
using Yozian.Extension.Test.Data.Entities;

namespace Yozian.Extension.Test
{

    public class PaginationExtensionTest
    {
        [OneTimeSetUp]
        public void SetUp()
        {

            var dbContext = new SouthSeaDbContext();

            var books = Enumerable.Range(1, 10)
                .Select(x => new Book()
                {
                    Name = x.ToString()
                });


            dbContext.AddRange(books);


            dbContext.SaveChanges();
        }

        [TestCase()]
        public void Test_Pagination()
        {
            var count = 10;
            var size = 3;
            var source = Enumerable
                .Range(1, count)
                .AsQueryable();

            var result = source.ToPagination(1, size);

            Assert.AreEqual(count, result.TotalCount);
            Assert.AreEqual(size, result.Records.Count());
            Assert.AreEqual(1, result.CurrentPage);
            Assert.AreEqual(4, result.PageCount);

        }


        [TestCase(1)]
        [TestCase(3)]
        [TestCase(5)]
        public void Test_Pagination_ToPageStart(int currentPage)
        {
            var count = 32;
            var size = 3;
            var source = Enumerable
                .Range(1, count)
                .AsQueryable();

            var result = source.ToPagination(currentPage, size);


            var pageSize = 5;

            Page<int> page = result.ToPage(pageSize);

            Assert.AreEqual(count, page.TotalCount);
            Assert.AreEqual(size, page.Records.Count());
            Assert.AreEqual(currentPage, page.CurrentPage);


            Assert.AreEqual((count / size) + (count % size == 0 ? 0 : 1), page.PageCount);

            var currentSize = currentPage == 11 ? count % size : size;
            Assert.AreEqual(currentSize, page.Records.Count());

            //
            Assert.AreEqual(5, page.PageSize);

            // 
            Assert.AreEqual(false, page.HasPreviosPages);
            Assert.AreEqual(1, page.PreviosLastPageNo);

            //
            Assert.AreEqual(true, page.HasNextPages);
            Assert.AreEqual(6, page.NextStartPageNo);

        }


        [TestCase(6)]
        [TestCase(9)]
        [TestCase(10)]
        public void Test_Pagination_ToPageMiddle(int currentPage)
        {
            var count = 32;
            var size = 3;
            var source = Enumerable
                .Range(1, count)
                .AsQueryable();

            var result = source.ToPagination(currentPage, size);


            var pageSize = 5;

            Page<int> page = result.ToPage(pageSize);

            Assert.AreEqual(count, page.TotalCount);
            Assert.AreEqual(size, page.Records.Count());
            Assert.AreEqual(currentPage, page.CurrentPage);


            Assert.AreEqual((count / size) + (count % size == 0 ? 0 : 1), page.PageCount);

            var currentSize = currentPage == 11 ? count % size : size;
            Assert.AreEqual(currentSize, page.Records.Count());

            //
            Assert.AreEqual(5, page.PageSize);

            // 
            Assert.AreEqual(true, page.HasPreviosPages);
            Assert.AreEqual(5, page.PreviosLastPageNo);

            //
            Assert.AreEqual(true, page.HasNextPages);
            Assert.AreEqual(11, page.NextStartPageNo);

        }

        [TestCase(11)]
        public void Test_Pagination_ToPageEnd(int currentPage)
        {
            var count = 32;
            var size = 3;
            var source = Enumerable
                .Range(1, count)
                .AsQueryable();

            var result = source.ToPagination(currentPage, size);


            var pageSize = 5;

            Page<int> page = result.ToPage(pageSize);

            Assert.AreEqual(count, page.TotalCount);
            Assert.AreEqual(count % size, page.Records.Count());
            Assert.AreEqual(currentPage, page.CurrentPage);


            Assert.AreEqual((count / size) + (count % size == 0 ? 0 : 1), page.PageCount);

            var currentSize = currentPage == 11 ? count % size : size;
            Assert.AreEqual(currentSize, page.Records.Count());

            //
            Assert.AreEqual(5, page.PageSize);

            // 
            Assert.AreEqual(true, page.HasPreviosPages);
            Assert.AreEqual(10, page.PreviosLastPageNo);

            //
            Assert.AreEqual(false, page.HasNextPages);
            Assert.AreEqual(11, page.NextStartPageNo);

        }

        [TestCase(11)]
        public void Test_Pagination_ToPageWithConverter(int currentPage)
        {
            var count = 32;
            var size = 3;
            var source = Enumerable
                .Range(1, count)
                .AsQueryable();

            var result = source.ToPagination(currentPage, size);


            var pageSize = 5;

            Page<string> page = result.ToPage(pageSize, x => x.ToString());


            Assert.AreEqual(count, page.TotalCount);
            Assert.AreEqual(count % size, page.Records.Count());
            Assert.AreEqual(currentPage, page.CurrentPage);


            Assert.AreEqual((count / size) + (count % size == 0 ? 0 : 1), page.PageCount);

            var currentSize = currentPage == 11 ? count % size : size;
            Assert.AreEqual(currentSize, page.Records.Count());

            //
            Assert.AreEqual(5, page.PageSize);

            // 
            Assert.AreEqual(true, page.HasPreviosPages);
            Assert.AreEqual(10, page.PreviosLastPageNo);

            //
            Assert.AreEqual(false, page.HasNextPages);
            Assert.AreEqual(11, page.NextStartPageNo);

        }


        [TestCase()]
        public void Test_PaginationWithConverter()
        {
            var count = 10;
            var size = 3;
            var source = Enumerable
                .Range(1, count)
                .AsQueryable();

            var result = source.ToPagination(1, size, x => x.ToString());

            Assert.AreEqual(count, result.TotalCount);
            Assert.AreEqual(size, result.Records.Count());
            Assert.AreEqual(typeof(string), result.Records.First().GetType());
            Assert.AreEqual(1, result.CurrentPage);
            Assert.AreEqual(4, result.PageCount);

        }

        [TestCase()]
        public void Test_PaginationNextPage()
        {
            var count = 10;
            var size = 3;
            var source = Enumerable
                .Range(1, count)
                .AsQueryable();

            var result = source.ToPagination(1, size);
            var fetchCount = 1;
            var startValue = result.Records.First();
            do
            {
                // the first value in the next page should be 
                Assert.AreEqual(startValue + (size * (fetchCount - 1)), result.Records.First());

                result.FetchNextPage();

                fetchCount++;
            }
            while (result.HasNextPage);

            Assert.AreEqual(fetchCount, result.PageCount);


        }


        [TestCase()]
        public void Test_PaginationForEfCore()
        {

            var db = new SouthSeaDbContext();
            var count = 10;
            var size = 3;

            var result = db.Books.ToPagination(1, size);

            Assert.AreEqual(count, result.TotalCount);
            Assert.AreEqual(size, result.Records.Count());
            Assert.AreEqual(1, result.CurrentPage);
            Assert.AreEqual(4, result.PageCount);

        }


        [TestCase()]
        public void Test_PaginationHasNextPageForEfCore()
        {

            var db = new SouthSeaDbContext();
            var size = 3;

            var fetchCount = 1;

            var result = db.Books.ToPagination(1, size);

            do
            {
                // process records here
                result.Records.ForEach(it =>
                {
                    // do somthing

                });

                result.FetchNextPage();



                fetchCount++;
            }
            while (result.HasNextPage);


            Assert.AreEqual(10, result.Records.Last().Id);

            Assert.AreEqual(4, fetchCount);

        }

        [TestCase()]
        public async Task Test_PaginationAsyncForEfCore()
        {

            var db = new SouthSeaDbContext();
            var size = 3;

            var fetchCount = 1;

            var result = await db.Books.ToPaginationAsync(1, size);

            do
            {
                // process records here
                result.Records.ForEach(it =>
                {
                    // do somthing

                });

                await result.FetchNextPageAsync();

                fetchCount++;
            } while (result.HasNextPage);


            Assert.AreEqual(10, result.Records.Last().Id);

            Assert.AreEqual(4, fetchCount);

        }

    }
}