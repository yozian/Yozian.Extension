using System;
using System.Linq;
using System.Reflection;

namespace Yozian.Extension;

/// <summary>
/// 
/// </summary>
public static class EnumExtension
{
    extension(Enum @this)
    {
        public T GetAttributeOf<T>()
            where T : Attribute
        {
            return @this.GetType().GetMember(@this.ToString()).First().GetCustomAttribute<T>();
        }
    }
}