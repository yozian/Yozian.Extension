using NUnit.Framework;
using Yozian.Extension.Test.TestMaterial;

namespace Yozian.Extension.Test
{
    public class ObjectExtensionTest
    {
        [SetUp]
        public void Setup()
        {
        }

        [TestCase]
        public void Test_SafeToString()
        {
            object obj = null;

            Assert.AreEqual(string.Empty, obj.SafeToString());
        }


        [TestCase]
        public void Test_ConvertAll()
        {
            var man = new Person()
            {
                Name = "Yozian  "
            };

            man.ConvertAll((string p) => p.Trim());

            Assert.AreEqual("Yozian", man.Name);
        }


        [TestCase]
        public void Test_ShallowClone()
        {
            var man = new Person()
            {
                Name = "Yozian"
            };

            var cloneMan = man.ShallowClone();

            Assert.AreEqual(man.Name, cloneMan.Name);

            Assert.AreNotEqual(man.GetHashCode(), cloneMan.GetHashCode());
        }
    }
}
