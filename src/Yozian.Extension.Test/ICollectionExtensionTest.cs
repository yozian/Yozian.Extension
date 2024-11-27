using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Yozian.Extension.Dtos;
using Yozian.Extension.Test.Data.Entities;

namespace Yozian.Extension.Test;

public class ICollectionExtensionTest
{
    [SetUp]
    public void Setup()
    {
    }

    [TestCase]
    public void Test_AddWhen()
    {
        var collection = new List<int>
        {
            1,
            2,
            3
        };

        collection.AddWhen(false, 4);

        Assert.AreEqual(3, collection.Count);

        collection.AddWhen(true, 4);

        Assert.AreEqual(4, collection.Count);
    }

    [TestCase]
    public void Test_RemoveWhen()
    {
        var collection = new List<int>
        {
            1,
            2,
            3
        };

        collection.RemoveWhen(false, 1);

        Assert.AreEqual(3, collection.Count);

        collection.RemoveWhen(true, 1);

        Assert.AreEqual(2, collection.Count);
    }


    [TestCase]
    public void Test_Comparer()
    {
        var sourceA = Enumerable
            .Range(1, 7)
            .Select(i => new KeyValuePair<string, int>(i.ToString(), i))
            .ToList();

        var sourceB = Enumerable
            .Range(1, 5)
            .Select(i => new KeyValuePair<string, int>(i.ToString(), i))
            .ToList();

        var excludedList = sourceA
            .Except(
                sourceB,
                new GenericComparer<KeyValuePair<string, int>>((x, y) => x.Key == y.Key)
            )
            .ToList();

        Assert.AreEqual(2, excludedList.Count);

        Assert.AreEqual("6,7", string.Join(",", excludedList.Select(x => x.Key)));
    }

    [TestCase]
    public void Test_ObjectComparer()
    {
        var sourceA = Enumerable
            .Range(1, 7)
            .Select(
                i => new Book
                {
                    Id = i,
                    Name = i.ToString()
                }
            )
            .ToList();

        var sourceB = Enumerable
            .Range(1, 5)
            .Select(
                i => new Book
                {
                    Id = i,
                    Name = i.ToString()
                }
            )
            .ToList();

        var excludedList = sourceA
            .Except(
                sourceB,
                new GenericComparer<Book>((x, y) => x.Id == y.Id)
            )
            .ToList();

        Assert.AreEqual(2, excludedList.Count);

        Assert.AreEqual("6,7", string.Join(",", excludedList.Select(x => x.Id)));
    }

    [TestCase]
    public void Test_TwoValueTypeListsDiffer()
    {
        var listA = new List<int>
        {
            1,
            2,
            3,
            4,
            5
        };

        var listB = new List<int>
        {
            4,
            5,
            6,
            7,
            8
        };

        var differResult = listA.DifferFrom(listB, new GenericComparer<int>((a, b) => a == b));

        // asserts
        Assert.AreEqual(3, differResult.SourceMissingItems.Count);
        Assert.AreEqual(3, differResult.TargetMissingItems.Count);
        Assert.AreEqual(2, differResult.MatchedItems.Count);
    }


    [TestCase]
    public void Test_TwoRefTypeListsDiffer()
    {
        var listA = Enumerable
            .Range(1, 5)
            .Select(
                i => new Book
                {
                    Id = i,
                    Name = $"Book {i}"
                }
            )
            .ToList();

        var listB = Enumerable
            .Range(4, 5)
            .Select(
                i => new Book
                {
                    Id = i,
                    Name = $"Book {i}"
                }
            )
            .ToList();

        var differResult = listA.DifferFrom(
            listB,
            new GenericComparer<Book>((a, b) => a.Id == b.Id)
        );

        // asserts
        Assert.AreEqual(3, differResult.SourceMissingItems.Count);
        Assert.AreEqual(3, differResult.TargetMissingItems.Count);
        Assert.AreEqual(2, differResult.MatchedItems.Count);
    }

    [TestCase]
    public void Test_TwoListsLeftOuterJoin()
    {
        // with two object lists
        var listA = new List<Book>
        {
            new Book
            {
                Id = 1,
                Name = "Book 1"
            },
            new Book
            {
                Id = 2,
                Name = "Book 2"
            },
            new Book
            {
                Id = 3,
                Name = "Book 3"
            }
        };

        var listB = new List<Book>
        {
            new Book
            {
                Id = 2,
                Name = "Book 2"
            },
            new Book
            {
                Id = 3,
                Name = "Book 3"
            },
            new Book
            {
                Id = 4,
                Name = "Book 4"
            }
        };

        var composite = listA.LeftOuterJoin(
            listB,
            a => a.Id,
            b => b.Id,
            (a, b) => new
            {
                A = a,
                B = b
            }
        );

        // asserts
        Assert.AreEqual(3, composite.Count());
        Assert.AreEqual(2, composite.Count(x => x.B != null));
        Assert.AreEqual(1, composite.Count(x => x.B == null));
    }
}
