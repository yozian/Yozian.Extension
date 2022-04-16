using System.Collections.Generic;
using NUnit.Framework;

namespace Yozian.Extension.Test
{
    public class IDictionaryExtensionTest
    {
        private readonly IDictionary<string, string> hash = new Dictionary<string, string>();

        [SetUp]
        public void Setup()
        {
        }

        [TestCase]
        public void Test_SafeGet()
        {
            var key = "Key";
            var str = this.hash.SafeGet(key);

            Assert.AreEqual(null, str);

            this.hash.Add(key, "val");

            Assert.AreEqual("val", this.hash.SafeGet(key));
        }
    }
}
