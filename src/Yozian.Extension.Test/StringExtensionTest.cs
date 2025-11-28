using System;
using NUnit.Framework;
using Yozian.Extension.Test.TestMaterial;

namespace Yozian.Extension.Test;

public class StringExtensionTest
{
    [SetUp]
    public void Setup()
    {
    }

    [TestCase]
    public void Test_ToEnum()
    {
        var category = Category.Ship.ToString().ToEnum<Category>();

        Assert.AreEqual(Category.Ship, category);
    }

    [TestCase]
    public void Test_ToEnum_Null()
    {
        var category = "ShipX".ToEnum<Category>();

        Assert.AreEqual(Category.Car, category);
    }

    [Test]
    public void Test_ToEnum_WithNullInput()
    {
        string categoryName = null;

        var category = categoryName.ToEnum<Category>();

        Assert.AreEqual(default(Category), category);
    }

    [TestCase]
    public void Test_ToEnum_NullWithOutDefatul()
    {
        Assert.Throws(
            typeof(ArgumentException),
            () =>
            {
                var category = "ShipX".ToEnum<Category>(false);
            }
        );
    }


    [TestCase(-10)]
    [TestCase(0)]
    [TestCase(3)]
    [TestCase(13)]
    public void Test_LimitLength(int len)
    {
        var text = "1234567890";

        var result = text.LimitLength(len);

        if (len <= 0)
        {
            Assert.AreEqual(string.Empty, result);
        }
        else
        {
            Assert.AreEqual(Math.Min(len, text.Length), result.Length);
        }
    }


    [TestCase(0)]
    [TestCase(1)]
    [TestCase(3)]
    public void Test_Repeat(int count)
    {
        var text = "abc";

        var result = text.Repeat(count);

        Assert.AreEqual(text.Length * count, result.Length);
    }


    [TestCase("test1")]
    [TestCase("1234")]
    [TestCase("!@#$")]
    public void Test_Base64EncodeAndDecode(string text)
    {
        var encodedText = text.EncodeToBase64();

        var decodedText = encodedText.DecodeBase64Text();

        Assert.AreEqual(text, decodedText);
    }

    [Test]
    public void Test_Base64Encode_WithNullOrEmpty()
    {
        string text = null;
        Assert.IsNull(text.EncodeToBase64());

        text = string.Empty;
        Assert.AreEqual(string.Empty, text.EncodeToBase64());
    }

    [Test]
    public void Test_Base64Decode_WithNullOrEmpty()
    {
        string text = null;
        Assert.IsNull(text.DecodeBase64Text());

        text = string.Empty;
        Assert.AreEqual(string.Empty, text.DecodeBase64Text());
    }

    [Test]
    public void Test_DecodeBase64Text_WithInvalidInput()
    {
        Assert.Throws<FormatException>(() => "Invalid$".DecodeBase64Text());
    }
}
