using System;
using NUnit.Framework;

namespace Yozian.Extension.Test;

[TestFixture]
public class VersionExtensionTest
{
    [Test]
    public void Test_VersionIncrement()
    {
        var version = new Version(
            1,
            2,
            3
        );

        var newVersion = version.IncreaseBuild();
        Assert.AreEqual(1, newVersion.Major);
        Assert.AreEqual(2, newVersion.Minor);
        Assert.AreEqual(4, newVersion.Build);

        newVersion = version.IncreaseMinor();
        Assert.AreEqual(1, newVersion.Major);
        Assert.AreEqual(3, newVersion.Minor);
        Assert.AreEqual(0, newVersion.Build);

        newVersion = version.IncreaseMajor();
        Assert.AreEqual(2, newVersion.Major);
        Assert.AreEqual(0, newVersion.Minor);
        Assert.AreEqual(0, newVersion.Build);
    }
}
