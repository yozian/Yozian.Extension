using System;
using NUnit.Framework;

namespace Yozian.Extension.Test;

[TestFixture]
public class GuidExtensionTest
{
    [Test]
    public void Test_IsNullOrEmpty()
    {
        Guid? guid = null;

        Assert.IsTrue(guid.IsNullOrEmpty());

        guid = Guid.Empty;

        Assert.IsTrue(guid.IsNullOrEmpty());

        guid = Guid.NewGuid();

        Assert.IsFalse(guid.IsNullOrEmpty());
    }

    [Test]
    public void Test_IsEmpty()
    {
        var guid = Guid.Empty;

        Assert.IsTrue(guid.IsEmpty());

        guid = Guid.NewGuid();

        Assert.IsFalse(guid.IsEmpty());
    }

    [Test]
    public void Test_Increment()
    {
        var guid = Guid.NewGuid();

        // increment 1000 times, make it sure it won't overflow
        for (var i = 0; i < 1000; i++)
        {
            var incrementedGuid = guid.Increment();

            Assert.AreNotEqual(guid, incrementedGuid);
            //
            guid = incrementedGuid;
        }
    }
}