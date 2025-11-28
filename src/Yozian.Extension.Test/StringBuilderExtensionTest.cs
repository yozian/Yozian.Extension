using System;
using System.Text;
using NUnit.Framework;

namespace Yozian.Extension.Test;

public class StringBuilderExtensionTest
{
    [SetUp]
    public void Setup()
    {
    }

    [TestCase]
    public void Test_AppendWhenFalse()
    {
        var sb = new StringBuilder();
        var template = "won't be concated!";
        sb.AppendWhen(false, template);

        Assert.AreEqual(string.Empty, sb.ToString());
    }

    [TestCase]
    public void Test_AppendWhenTrue()
    {
        var sb = new StringBuilder();
        var template = "it's me!";
        sb.AppendWhen(true, template);

        Assert.AreEqual(template, sb.ToString());
    }

    [TestCase]
    public void Test_AppendWhenFalseWithArgs()
    {
        var sb = new StringBuilder();
        var template = "won't be concated!{0},{1}";
        sb.AppendWhen(
            false,
            template,
            "a",
            "b"
        );

        Assert.AreEqual(string.Empty, sb.ToString());
    }

    [TestCase]
    public void Test_AppendWhenTrueWithArgs()
    {
        var sb = new StringBuilder();
        var template = "hi, {0},{1}";
        sb.AppendWhen(
            true,
            template,
            "a",
            "b"
        );

        Assert.AreEqual(
            string.Format(
                template,
                "a",
                "b"
            ),
            sb.ToString()
        );
    }

    [Test]
    public void Test_AppendLineWhenTrue()
    {
        var sb = new StringBuilder();
        sb.AppendLineWhen(true, "line");

        Assert.AreEqual($"line{Environment.NewLine}", sb.ToString());
    }

    [Test]
    public void Test_AppendLineWhenFalse()
    {
        var sb = new StringBuilder();
        sb.AppendLineWhen(false, "line");

        Assert.AreEqual(string.Empty, sb.ToString());
    }

    [Test]
    public void Test_AppendLineWhenTrueWithArgs()
    {
        var sb = new StringBuilder();
        sb.AppendLineWhen(
            true,
            "{0}/{1}",
            "a",
            "b"
        );

        Assert.AreEqual($"a/b{Environment.NewLine}", sb.ToString());
    }
}
