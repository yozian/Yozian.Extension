using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NUnit.Framework;

namespace Yozian.Extension.Test.Tool;

public class CreateMethodMapTool
{
    private IEnumerable<Type> types = new List<Type>();

    [SetUp]
    public void Setup()
    {
    }

    [TestCase]
    public void Test_CreateMehtodMap()
    {
        var type = typeof(ExceptionExtension);

        var testProjectUrl = "https://github.com/yozian/Yozian.Extension/blob/master/src/Yozian.Extension.Test/";

        type.Assembly
            .ExportedTypes
            .Where(
                et => et.Attributes
                      == (TypeAttributes.Public
                          | TypeAttributes.Abstract
                          | TypeAttributes.Sealed
                          | TypeAttributes.BeforeFieldInit)
            )
            .ForEach(
                t =>
                {
                    Console.WriteLine($"* [{t.Name}]({testProjectUrl}/{t.Name}Test.cs)");
                    Console.WriteLine("");

                    t.GetMethods()
                        .Where(m => m.DeclaringType == t)
                        .DistinctBy(m => m.Name)
                        .ForEach(
                            m =>
                            {
                                Console.WriteLine($"    >{m.Name}");
                                Console.WriteLine("");
                            }
                        );
                }
            );
    }
}
