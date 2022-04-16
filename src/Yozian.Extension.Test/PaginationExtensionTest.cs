using System;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NUnit.Framework;
using Yozian.Extension.Pagination;
using Yozian.Extension.Test.Data;
using Yozian.Extension.Test.Data.Entities;

namespace Yozian.Extension.Test
{
    public class PaginationExtensionTest
    {
        private static readonly int totalBooksCount = 10;

        [OneTimeSetUp]
        public void SetUp()
        {
            var dbContext = new SouthSeaDbContext();

            var books = Enumerable.Range(1, totalBooksCount)
               .Select(
                    x => new Book()
                    {
                        Name = x.ToString()
                    }
                );

            dbContext.AddRange(books);

            dbContext.SaveChanges();
        }

        [TestCase]
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

            var page = result.ToPage(pageSize);

            Console.WriteLine(JsonConvert.SerializeObject(page));

            Assert.AreEqual(count, page.TotalCount);
            Assert.AreEqual(size, page.Records.Count());
            Assert.AreEqual(currentPage, page.CurrentPage);

            Assert.AreEqual(count / size + (count % size == 0 ? 0 : 1), page.PageCount);

            var currentSize = currentPage == 11 ? count % size : size;
            Assert.AreEqual(currentSize, page.Records.Count());

            //
            Assert.AreEqual(5, page.PageSize);

            // 
            Assert.AreEqual(false, page.HasPreviousPages);
            Assert.AreEqual(1, page.PreviousLastPageNo);

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

            var page = result.ToPage(pageSize);

            Assert.AreEqual(count, page.TotalCount);
            Assert.AreEqual(size, page.Records.Count());
            Assert.AreEqual(currentPage, page.CurrentPage);

            Assert.AreEqual(count / size + (count % size == 0 ? 0 : 1), page.PageCount);

            var currentSize = currentPage == 11 ? count % size : size;
            Assert.AreEqual(currentSize, page.Records.Count());

            //
            Assert.AreEqual(5, page.PageSize);

            // 
            Assert.AreEqual(true, page.HasPreviousPages);
            Assert.AreEqual(5, page.PreviousLastPageNo);

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

            var page = result.ToPage(pageSize);

            Assert.AreEqual(count, page.TotalCount);
            Assert.AreEqual(count % size, page.Records.Count());
            Assert.AreEqual(currentPage, page.CurrentPage);

            Assert.AreEqual(count / size + (count % size == 0 ? 0 : 1), page.PageCount);

            var currentSize = currentPage == 11 ? count % size : size;
            Assert.AreEqual(currentSize, page.Records.Count());

            //
            Assert.AreEqual(5, page.PageSize);

            // 
            Assert.AreEqual(true, page.HasPreviousPages);
            Assert.AreEqual(10, page.PreviousLastPageNo);

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

            var page = result.MapTo(x => x.ToString()).ToPage(pageSize);

            Assert.AreEqual(count, page.TotalCount);
            Assert.AreEqual(count % size, page.Records.Count());
            Assert.AreEqual(currentPage, page.CurrentPage);

            Assert.AreEqual(count / size + (count % size == 0 ? 0 : 1), page.PageCount);

            var currentSize = currentPage == 11 ? count % size : size;
            Assert.AreEqual(currentSize, page.Records.Count());

            //
            Assert.AreEqual(5, page.PageSize);

            // 
            Assert.AreEqual(true, page.HasPreviousPages);
            Assert.AreEqual(10, page.PreviousLastPageNo);

            //
            Assert.AreEqual(false, page.HasNextPages);
            Assert.AreEqual(11, page.NextStartPageNo);
        }


        [TestCase]
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


        [TestCase]
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


        [TestCase]
        public void Test_PaginationHasNextPageForEfCore()
        {
            var db = new SouthSeaDbContext();
            var size = 3;

            var page = 2;

            var result = db.Books.ToPagination(page, size);

            Assert.AreEqual(size, result.Records.Count());

            Assert.AreEqual(result.TotalCount, totalBooksCount);
        }

        [TestCase]
        public async Task Test_PaginationAsyncForEfCore()
        {
            var db = new SouthSeaDbContext();
            var size = 6;

            var result = await db.Books.ToPaginationAsync(2, size);

            Assert.AreEqual(10, result.Records.Last().Id);
        }


        [TestCase]
        public void Test_ToPagination()
        {
            var count = 100;
            var list = Enumerable.Range(1, count).ToList();
            var size = 5;

            var result = list.ToPagination(size);

            var page = result.First();

            Assert.AreEqual(count, page.TotalCount);
            Assert.AreEqual(size, page.Size);
            Assert.AreEqual(size, page.Records.Count());
            Assert.AreEqual(1, page.CurrentPage);

            var lastPage = result.Last();

            Assert.AreEqual(list.Last(), lastPage.Records.Last());
        }
    }
}
