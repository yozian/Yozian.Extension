using System;
using NUnit.Framework;

namespace Yozian.Extension.Test;

[TestFixture]
public class EnumExtensionTest
{
    public class ActualNameAttribute : Attribute
    {
        public string Name { get; set; }
    }

    public enum TestEnum
    {
        [ActualName(Name = "Secrete")]
        SpiderMan,
    }


    [Test]
    public void ShouldRetrieveAttributeValueSuccess()
    {
        var en = TestEnum.SpiderMan;

        var attr = en.GetAttributeOf<ActualNameAttribute>();

        Assert.NotNull(attr);

        Assert.AreEqual("Secrete", attr.Name);
    }
}