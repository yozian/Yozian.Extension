using NUnit.Framework;
using Yozian.Extension.Test.TestMaterial;

namespace Yozian.Extension.Test;

public class ObjectExtensionTest
{
    [SetUp]
    public void Setup()
    {
    }

    [TestCase]
    public void Test_SafeToString()
    {
        object obj = null;

        Assert.AreEqual(string.Empty, obj.SafeToString());
    }


    [Test]
    public void Test_SafeToString_WithValue()
    {
        object obj = 123;

        Assert.AreEqual("123", obj.SafeToString());
    }


    [TestCase]
    public void Test_ConvertAll()
    {
        var man = new Person
        {
            Name = "Yozian  ",
            Nickname = string.Empty,
        };

        man.ConvertAll((string p) => p.Trim());

        Assert.AreEqual("Yozian", man.Name);
    }

    [Test]
    public void Test_ConvertAll_WithMultipleStringProperties()
    {
        var target = new Person
        {
            Name = "  name  ",
            Nickname = null,
        };

        target.ConvertAll((string value) => string.IsNullOrWhiteSpace(value) ? "N/A" : value.Trim());

        Assert.AreEqual("name", target.Name);
        Assert.AreEqual("N/A", target.Nickname);
    }


    [TestCase]
    public void Test_ShallowClone()
    {
        var man = new Person
        {
            Name = "Yozian",
        };

        var cloneMan = man.ShallowClone();

        Assert.AreEqual(man.Name, cloneMan.Name);

        Assert.AreNotEqual(man.GetHashCode(), cloneMan.GetHashCode());
    }

    [Test]
    public void Test_ShallowClone_ModifyingCloneDoesNotChangeSource()
    {
        var person = new Person
        {
            Name = "Original",
            Age = 18,
        };

        var clone = person.ShallowClone();
        clone.Name = "Clone";

        Assert.AreEqual("Original", person.Name);
        Assert.AreEqual("Clone", clone.Name);
    }
}
