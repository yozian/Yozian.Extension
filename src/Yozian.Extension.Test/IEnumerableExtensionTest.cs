using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Yozian.Extension.Test.TestMaterial;

namespace Yozian.Extension.Test
{
    public class IEnumerableExtensionTest
    {
        [SetUp]
        public void Setup()
        {
        }

        [TestCase()]
        public void Test_ForEach()
        {
            var list = Enumerable.Range(1, 5);

            list.ForEach(x =>
            {
                Console.Write(x);
            });


            Assert.Pass();
        }


        [TestCase()]
        public void Test_ForEachWithIndex()
        {
            var list = Enumerable.Range(1, 5);

            list.ForEach((x, index) =>
            {
                Console.Write($"{x}({index})");
            });

            Assert.Pass();
        }


        [TestCase()]
        public void Test_FlattenToString()
        {
            var list = Enumerable.Range(1, 5).Select(x => x.ToString());

            var result = list.FlattenToString();

            Assert.AreEqual("12345", result);
        }

        [TestCase()]
        public void Test_FlattenToStringWithConverter()
        {
            var list = Enumerable.Range(1, 5);

            var result = list.FlattenToString(x => (x * 2).ToString());

            Assert.AreEqual("246810", result);
        }


        [TestCase()]
        public void Test_DistinctBy()
        {
            var list = new List<Person>()
            {
                new Person(){
                    Name = "A",
                    Age = 10
                },
                new Person(){
                    Name = "B",
                    Age = 10
                },
                new Person(){
                    Name = "C",
                    Age = 11
                },
                    new Person(){
                    Name = "C",
                    Age = 11
                },

            };

            var byName = list.DistinctBy(x => x.Name);
            Assert.AreEqual(3, byName.Count());

            var byAge = list.DistinctBy(x => x.Age);
            Assert.AreEqual(2, byAge.Count());

        }


        [TestCase(3)]
        [TestCase(5)]
        [TestCase(12)]
        public void Test_ToPagination(int total)
        {
            var limits = 5;
            var list = Enumerable.Range(0, total).ToList();

            var pagination = list.ToPagination(limits);

            var pageCount = 0;
            if (list.Count() % limits == 0)
            {
                pageCount = list.Count() / limits;
            }
            else
            {
                pageCount = list.Count() / limits + 1;
            }

            Assert.AreEqual(pageCount, pagination.PageCount);
            Assert.AreEqual(pageCount, pagination.Pages.Count());
            Assert.AreEqual(total, pagination.Pages.SelectMany(x => x.ToList()).Count());
            Assert.AreEqual(limits, pagination.Limits);
        }

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


       
    }
}