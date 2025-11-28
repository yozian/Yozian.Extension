using System;

namespace Yozian.Extension;

/// <summary>
/// 
/// </summary>
public static class GuidExtension
{
    extension(Guid? @this)
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool IsNullOrEmpty()
        {
            return @this == null || @this.Value.IsEmpty();
        }
    }

    extension(Guid @this)
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool IsEmpty()
        {
            return @this == Guid.Empty;
        }

        public Guid Increment()
        {
            var bytes = @this.ToByteArray();

            for (var i = bytes.Length - 1; i >= 0; i--)
            {
                if (++bytes[i] != 0)
                {
                    break;
                }
            }

            return new Guid(bytes);
        }
    }
}