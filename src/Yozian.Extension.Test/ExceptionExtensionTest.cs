using System;
using System.Threading.Tasks;
using NUnit.Framework;
using Yozian.Extension.Test.TestMaterial;

namespace Yozian.Extension.Test;

public class ExceptionExtensionTest
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public async Task Test_DumpDetail()
    {
        try
        {
            await Task.Run(() => new Person().Walk());
        }
        catch (Exception ex)
        {
            var st = ex.DumpDetail();
            Console.WriteLine(st);
        }
    }

    [Test]
    public void DumpDetail_ShouldIncludeInnerExceptionMessages()
    {
        try
        {
            ThrowNestedException();
        }
        catch (Exception ex)
        {
            var detail = ex.DumpDetail();

            Assert.That(detail, Does.Contain("Outer boom"));
            Assert.That(detail, Does.Contain("Inner boom"));
        }
    }

    [Test]
    public void DumpDetail_ShouldHonorFrameFilter()
    {
        try
        {
            ThrowNestedException();
        }
        catch (Exception ex)
        {
            var filtered = ex.DumpDetail(frame => frame.GetMethod()?.DeclaringType == typeof(ExceptionExtensionTest));

            Assert.That(filtered, Does.Contain(nameof(ExceptionExtensionTest)));

            var stripped = ex.DumpDetail(_ => false);

            Assert.False(stripped.Contains("class:"), "Filter should remove stack frame details");
        }
    }

    private static void ThrowNestedException()
    {
        try
        {
            throw new InvalidOperationException("Inner boom");
        }
        catch (Exception ex)
        {
            throw new ApplicationException("Outer boom", ex);
        }
    }
}
