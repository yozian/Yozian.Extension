using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Yozian.Extension.CollectionDto;

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
            var collection = new List<int>() {1, 2, 3};

            collection.AddWhen(false, 4);

            Assert.AreEqual(3, collection.Count);

            collection.AddWhen(true, 4);

            Assert.AreEqual(4, collection.Count);
        }

        [TestCase()]
        public void Test_RemoveWhen()
        {
            var collection = new List<int>() {1, 2, 3};

            collection.RemoveWhen(false, 1);

            Assert.AreEqual(3, collection.Count);

            collection.RemoveWhen(true, 1);

            Assert.AreEqual(2, collection.Count);
        }


        [TestCase()]
        public void Test_Comparer()
        {
            var sourceA = Enumerable
                .Range(1, 7)
                .Select(i => new KeyValuePair<string, int>(i.ToString(), i))
                .ToList();


            var sourceB = Enumerable
                .Range(1, 5)
                .Select(i => new KeyValuePair<string, int>(i.ToString(), i))
                .ToList();


            var excludedList = sourceA
                .Except(
                    sourceB,
                    new GenericComparer<KeyValuePair<string, int>>((x, y) => x.Key == y.Key)
                )
                .ToList();


            Assert.AreEqual(2, excludedList.Count);

            Assert.AreEqual("6,7", string.Join(",", excludedList.Select(x => x.Key)));
        }
    }
}
