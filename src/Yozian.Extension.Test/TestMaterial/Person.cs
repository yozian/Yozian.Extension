using System;

namespace Yozian.Extension.Test.TestMaterial;

internal class Person
{
    public string Name { get; set; }

    public string Nickname { get; set; }

    public int Age { get; set; }


    public void Walk()
    {
        throw new Exception("Opps", new Exception("InnerException-Opps"));
    }
}
