using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using Yozian.Extension.Dtos;
using Yozian.Extension.Test.TestMaterial;

namespace Yozian.Extension.Test;

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

        list.ForEach(x =>
            {
                Console.Write(x);
            }
        );

        Assert.Pass();
    }

    [Test]
    public void Test_ForEach_AllowsMutationWhenUsingClone()
    {
        var list = new List<int>
        {
            1,
            2,
        };

        list.ForEach(
            item =>
            {
                if (item == 1)
                {
                    list.Remove(item);
                }
            },
            true
        );

        CollectionAssert.AreEquivalent(
            new[]
            {
                2,
            },
            list
        );
    }


    [TestCase]
    public void Test_ForEachWithIndex()
    {
        var list = Enumerable.Range(1, 5);

        list.ForEach((x, index) =>
            {
                Console.Write($"{x}({index})");
            }
        );

        Assert.Pass();
    }

    [Test]
    public void Test_ForEachWithIndex_SequentialIndexes()
    {
        var list = new List<string>
        {
            "a",
            "b",
        };

        var indexes = new List<int>();

        list.ForEach((value, index) => { indexes.Add(index); });

        CollectionAssert.AreEqual(
            new[]
            {
                0,
                1,
            },
            indexes
        );
    }

    [Test]
    public async Task Test_ForEachAsync()
    {
        var list = Enumerable.Range(1, 3).ToList();
        var processed = new List<int>();

        await list.ForEachAsync(async (
                value,
                index,
                token
            ) =>
            {
                await Task.Delay(1, token);
                processed.Add(value + index);
            }
        );

        CollectionAssert.AreEqual(
            new[]
            {
                1,
                3,
                5,
            },
            processed
        );
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

    [Test]
    public void Test_FlattenToStringWithConverterAndSeparator()
    {
        var list = new[]
        {
            1,
            2,
            3,
        };

        var result = list.FlattenToString(x => x.ToString(), "-");

        Assert.AreEqual("1-2-3", result);
    }

    [Test]
    public void Test_FlattenToStringWithSeparator()
    {
        var list = new[]
        {
            "a",
            "b",
            "c",
        };

        var result = list.FlattenToString(",");

        Assert.AreEqual("a,b,c", result);
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


    [Test]
    public void Test_ForEachPage_PerfectDivision()
    {
        var limits = 5;
        var pageSizes = new List<int>();

        Enumerable
            .Range(1, 10)
            .ForEachPage(
                limits,
                (data, page) => { pageSizes.Add(data.Count()); }
            );

        CollectionAssert.AreEqual(
            new[]
            {
                5,
                5,
            },
            pageSizes
        );
    }


    [Test]
    public void Test_ForEachPage_EmptySource()
    {
        var invoked = false;

        Enumerable
            .Empty<int>()
            .ForEachPage(3, (_, _) => { invoked = true; });

        Assert.False(invoked);
    }


    [Test]
    public void Test_ForEachPage_InvalidLimits()
    {
        Assert.Throws<ArgumentException>(() => Enumerable
            .Range(1, 5)
            .ForEachPage(0, (_, _) => { })
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
            .Select(x => new Person
                {
                    Name = x.ToString(),
                    Age = 1,
                }
            )
            .ToList();

        var targets = Enumerable.Range(6, 10)
            .Select(x => new Person
                {
                    Name = x.ToString(),
                    Age = 1,
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

    [Test]
    public async Task Test_BatchProcessAsync()
    {
        var source = Enumerable.Range(1, 5);
        var batches = new List<List<int>>();

        var total = await source.BatchProcessAsync(
            2,
            async (items, token) =>
            {
                batches.Add(new List<int>(items));
                await Task.CompletedTask;
            }
        );

        Assert.AreEqual(5, total);
        Assert.AreEqual(3, batches.Count);
        CollectionAssert.AreEqual(
            new[]
            {
                1,
                2,
            },
            batches.First()
        );
    }

    [Test]
    public void Test_BatchProcessAsync_InvalidBatchSize()
    {
        Assert.ThrowsAsync<ArgumentException>(async () =>
            await Enumerable.Range(1, 5)
                .BatchProcessAsync(0, (items, token) => Task.CompletedTask)
        );
    }

    [Test]
    public void Test_BatchProcessAsync_NullConsumer()
    {
        Assert.ThrowsAsync<ArgumentNullException>(async () =>
            await Enumerable.Range(1, 5)
                .BatchProcessAsync(2, null)
        );
    }

    [Test]
    public void Test_BatchProcessAsync_SourceIsProducerConsumerCollection()
    {
        var queue = new ConcurrentQueue<int>(Enumerable.Range(1, 3));

        Assert.ThrowsAsync<ArgumentException>(async () =>
            await (queue as IEnumerable<int>).BatchProcessAsync(2, (items, token) => Task.CompletedTask)
        );
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
