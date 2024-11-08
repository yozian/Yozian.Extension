using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Yozian.Extension.Dtos;
using Yozian.Extension.Test.TestMaterial;

namespace Yozian.Extension.Test
{
    public class IEnumerableExtensionTest
    {
        [SetUp]
        public void Setup()
        {
        }

        [TestCase]
        public void Test_ForEach()
        {
            var list = Enumerable.Range(1, 5);

            list.ForEach(
                x =>
                {
                    Console.Write(x);
                }
            );

            Assert.Pass();
        }


        [TestCase]
        public void Test_ForEachWithIndex()
        {
            var list = Enumerable.Range(1, 5);

            list.ForEach(
                (x, index) =>
                {
                    Console.Write($"{x}({index})");
                }
            );

            Assert.Pass();
        }


        [TestCase]
        public void Test_FlattenToString()
        {
            var list = Enumerable.Range(1, 5).Select(x => x.ToString());

            var result = list.FlattenToString();

            Assert.AreEqual("12345", result);
        }

        [TestCase]
        public void Test_FlattenToStringWithConverter()
        {
            var list = Enumerable.Range(1, 5);

            var result = list.FlattenToString(x => (x * 2).ToString());

            Assert.AreEqual("246810", result);
        }


        // [TestCase(3)]
        // [TestCase(5)]
        // [TestCase(12)]
        // public void Test_ToPagination(int total)
        // {
        //     var limits = 5;
        //     var list = Enumerable.Range(0, total).ToList();
        //
        //     var pagination = list.ToPagination(limits);
        //
        //     var pageCount = 0;
        //     if (list.Count() % limits == 0)
        //     {
        //         pageCount = list.Count() / limits;
        //     }
        //     else
        //     {
        //         pageCount = list.Count() / limits + 1;
        //     }
        //
        //     Assert.AreEqual(pageCount, pagination.PageCount);
        //     Assert.AreEqual(pageCount, pagination.Pages.Count());
        //     Assert.AreEqual(total, pagination.Pages.SelectMany(x => x.ToList()).Count());
        //     Assert.AreEqual(limits, pagination.Limits);
        // }

        [TestCase(12)]
        public void Test_ForEachPage(int total)
        {
            var limits = 5;

            Enumerable
               .Range(0, total)
               .ForEachPage(
                    limits,
                    (data, page) =>
                    {
                        if (1 == page)
                        {
                            Assert.AreEqual(limits, data.Count());
                        }

                        if (3 == page)
                        {
                            Assert.AreEqual(2, data.Count());
                        }
                    }
                );
        }


        [TestCase]
        public void Test_ExceptGenericCompare()
        {
            var source = Enumerable.Range(1, 10);
            var targets = Enumerable.Range(6, 10);

            var results = source
               .Except(
                    targets,
                    new GenericComparer<int>((x, y) => x == y)
                )
               .ToList();

            Assert.AreEqual(5, results.Count());

            Assert.AreEqual(results.Last(), 5);
        }


        [TestCase]
        public void Test_ExceptLambadaCompare()
        {
            var source = Enumerable.Range(1, 10)
               .Select(
                    x => new Person
                    {
                        Name = x.ToString(),
                        Age = 1
                    }
                )
               .ToList();

            var targets = Enumerable.Range(6, 10)
               .Select(
                    x => new Person
                    {
                        Name = x.ToString(),
                        Age = 1
                    }
                );

            var results = source
               .Except(
                    targets,
                    (x, y) => x.Name == y.Name
                )
               .ToList();

            Assert.AreEqual(5, results.Count());

            Assert.AreEqual(results.Last().Name, "5");
        }


        [TestCase]
        public void Test_IsNullOrEmpty()
        {
            var source = new List<string>();

            Assert.IsTrue(source.IsNullOrEmpty());

            source.Add(string.Empty);

            Assert.IsFalse(source.IsNullOrEmpty());

            source = null;

            Assert.IsTrue(source.IsNullOrEmpty());
        }
    }
}
