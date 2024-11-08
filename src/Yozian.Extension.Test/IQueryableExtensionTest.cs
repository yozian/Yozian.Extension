using System.Linq;
using NUnit.Framework;

namespace Yozian.Extension.Test;

public class IQueryableExtensionTest
{
    [SetUp]
    public void Setup()
    {
    }

    [TestCase(true)]
    [TestCase(false)]
    public void Test_WhereWhen(bool condition)
    {
        var count = 100;
        var limit = 50;
        var list = Enumerable
            .Range(1, count)
            .AsQueryable()
            .WhereWhen(condition, x => x <= limit)
            .ToList();

        if (condition)
        {
            Assert.AreEqual(limit, list.Count);
        }
        else
        {
            Assert.AreEqual(count, list.Count);
        }
    }
}
