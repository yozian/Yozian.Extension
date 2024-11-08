using System;
using System.Linq;

namespace Yozian.Extension;

/// <summary>
/// 
/// </summary>
public static class GuidExtension
{
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public static bool IsNullOrEmpty(this Guid? @this)
    {
        return null == @this || @this.Value.IsEmpty();
    }


    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public static bool IsEmpty(this Guid @this)
    {
        return @this == Guid.Empty;
    }

    public static Guid Increment(this Guid @this)
    {
        var bytes = @this.ToByteArray();

        for (int i = bytes.Length - 1; i >= 0; i--)
        {
            if (++bytes[i] != 0)
            {
                break;
            }
        }

        return new Guid(bytes);
    }
}
