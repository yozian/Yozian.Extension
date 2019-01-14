using System;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using Yozian.Extension.Pagination;
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
            Assert.AreEqual(1, result.Page);
            Assert.AreEqual(4, result.PageCount);

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
            Assert.AreEqual(1, result.Page);
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
            Assert.AreEqual(1, result.Page);
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