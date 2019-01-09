using System;
using System.Threading.Tasks;
using NUnit.Framework;
using Yozian.Extension.Test.TestMaterial;

namespace Yozian.Extension.Test
{
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
    }
}