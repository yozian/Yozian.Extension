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

        [TestCase()]
        public void Test_SafeGet()
        {
            var key = "Key";
            var str = hash.SafeGet(key);

            Assert.AreEqual(null, str);

            hash.Add(key, "val");

            Assert.AreEqual("val", hash.SafeGet(key));
        }
    }
}