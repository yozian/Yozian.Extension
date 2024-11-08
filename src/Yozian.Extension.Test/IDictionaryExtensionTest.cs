using System.Collections.Generic;
using NUnit.Framework;

namespace Yozian.Extension.Test;

public class IDictionaryExtensionTest
{
    private IDictionary<string, string> hash;

    [SetUp]
    public void Setup()
    {
        this.hash = new Dictionary<string, string>
        {
            {
                "Key", "val"
            }
        };
    }

    [TestCase]
    public void Test_SafeGet()
    {
        var key = "NonExistentKey";
        var str = this.hash.SafeGet(key);

        Assert.AreEqual(null, str);

        Assert.AreEqual("val", this.hash.SafeGet("Key"));
    }

    [TestCase]
    public void Test_SafeGetCaseSensitive()
    {
        var key = "NonExistentKey";
        var str = this.hash.SafeGet(key);

        Assert.AreEqual(null, str);

        Assert.AreEqual("val", this.hash.SafeGet("Key", false));
    }

    [TestCase]
    public void Test_SafeGetCaseInsensitive()
    {
        var key = "NonExistentKey";
        var str = this.hash.SafeGet(key, true);

        Assert.AreEqual(null, str);

        Assert.AreEqual("val", this.hash.SafeGet("kEY", true));
    }

    [TestCase]
    public void Test_MergeDictionary()
    {
        var hash1 = new Dictionary<string, string>
        {
            {
                "key1", "val1"
            },
            {
                "key2", "val2"
            }
        };

        var hash2 = new Dictionary<string, string>
        {
            {
                "key2", "val2"
            },
            {
                "key3", "val3"
            }
        };

        hash1.MergeDictionary(hash2);

        Assert.AreEqual(3, hash1.Count);
        Assert.AreEqual("val1", hash1["key1"]);
        Assert.AreEqual("val2", hash1["key2"]);
        Assert.AreEqual("val3", hash1["key3"]);
    }
}
