using System;
using System.Linq.Expressions;
using NUnit.Framework;
using Yozian.Extension.Test.TestMaterial;

namespace Yozian.Extension.Test;

public class ExpressionExtensionTest
{
    [SetUp]
    public void Setup()
    {
    }

    [TestCase]
    public void Test_GetMemberName()
    {
        Expression<Func<Person, object>> expr = x => x.Name;

        var member = expr.GetMemberName();

        Assert.AreEqual(nameof(Person.Name), member);
    }

    [Test]
    public void GetMemberName_ShouldHandleValueTypeConversion()
    {
        Expression<Func<Person, object>> expr = x => x.Age;

        Assert.AreEqual(nameof(Person.Age), expr.GetMemberName());
    }

    [Test]
    public void GetMemberName_ShouldHandleMethodCalls()
    {
        Expression<Func<string, int>> expr = s => s.IndexOf("a", StringComparison.Ordinal);

        Assert.AreEqual(nameof(string.IndexOf), expr.GetMemberName());
    }

    [Test]
    public void GetMemberName_ShouldHandleParameterExpressions()
    {
        Expression<Func<Person, Person>> expr = person => person;

        Assert.AreEqual("person", expr.GetMemberName());
    }

    [Test]
    public void GetMemberName_ShouldHandleArrayLength()
    {
        Expression<Func<string[], object>> expr = arr => arr.Length;

        Assert.AreEqual("Length", expr.GetMemberName());
    }
}
