using System.Collections.Generic;
using NUnit.Framework;

namespace Yozian.Extension.Test
{
    public class ICollectionExtensionTest
    {


        [SetUp]
        public void Setup()
        {

        }

        [TestCase()]
        public void Test_AddWhen()
        {
            var collection = new List<int>() { 1, 2, 3 };

            collection.AddWhen(false, 4);

            Assert.AreEqual(3, collection.Count);

            collection.AddWhen(true, 4);

            Assert.AreEqual(4, collection.Count);

        }

        [TestCase()]
        public void RemoveWhen()
        {
            var collection = new List<int>() { 1, 2, 3 };

            collection.RemoveWhen(false, 1);

            Assert.AreEqual(3, collection.Count);

            collection.RemoveWhen(true, 1);

            Assert.AreEqual(2, collection.Count);
        }
    }
}