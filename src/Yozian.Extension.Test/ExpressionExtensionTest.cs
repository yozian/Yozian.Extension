using System;
using System.Linq.Expressions;
using NUnit.Framework;
using Yozian.Extension.Test.TestMaterial;

namespace Yozian.Extension.Test
{
    public class ExpressionExtensionTest
    {
        [SetUp]
        public void Setup()
        {
        }

        [TestCase()]
        public void Test_GetMemberName()
        {

            Expression<Func<Person, object>> expr = (Person x) => x.Name;

            var member = expr.GetMemberName();

            Assert.AreEqual(nameof(Person.Name), member);
        }
    }



}