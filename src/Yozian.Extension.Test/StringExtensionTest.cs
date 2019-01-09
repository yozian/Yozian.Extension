using System;
using NUnit.Framework;
using Yozian.Extension.Test.TestMaterial;

namespace Yozian.Extension.Test
{
    public class StringExtensionTest
    {
        [SetUp]
        public void Setup()
        {
        }

        [TestCase()]
        public void Test_ToEnum()
        {
            var category = Category.Ship.ToString().ToEnum<Category>();

            Assert.AreEqual(Category.Ship, category);
        }

        [TestCase()]
        public void Test_ToEnum_Null()
        {
            var category = "ShipX".ToEnum<Category>();

            Assert.AreEqual(Category.Car, category);
        }

        [TestCase()]
        public void Test_ToEnum_NullWithOutDefatul()
        {
            Assert.Throws(typeof(ArgumentException), () =>
            {
                var category = "ShipX".ToEnum<Category>(false);
            });
        }


        [TestCase(-10)]
        [TestCase(0)]
        [TestCase(3)]
        [TestCase(13)]
        public void Test_LimitLength(int len)
        {
            var text = "1234567890";

            var result = text.LimitLength(len);

            if (len <= 0)
            {
                Assert.AreEqual(string.Empty, result);
            }
            else
            {
                Assert.AreEqual(Math.Min(len, text.Length), result.Length);
            }

        }


        [TestCase(0)]
        [TestCase(1)]
        [TestCase(3)]
        public void Test_Repeate(int count)
        {
            var text = "abc";

            var result = text.Repeate(count);

            Assert.AreEqual(text.Length * count, result.Length);
        }


    }



}