using System;
using System.Collections.Generic;
using System.Text;

namespace Yozian.Extension.Test.TestMaterial
{
    internal class Person
    {

        public string Name { get; set; }

        public int Age { get; set; }


        public void Walk()
        {
            throw new Exception("Opps", new Exception("InnerException-Opps"));
        }
    }
}
