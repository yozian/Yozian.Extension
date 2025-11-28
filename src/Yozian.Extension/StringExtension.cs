using System;
using System.Linq;
using System.Text;

namespace Yozian.Extension;

public static class StringExtension
{
    extension(string @this)
    {
        public T ToEnum<T>(bool enableFailOverWithDefaultValue = true)
        {
            try
            {
                return (T)Enum.Parse(
                    typeof(T),
                    @this,
                    true
                );
            }
            catch (Exception)
            {
                if (!enableFailOverWithDefaultValue)
                {
                    throw;
                }

                return default;
            }
        }

        public string LimitLength(int length)
        {
            if (length <= 0)
            {
                return string.Empty;
            }

            if (@this.Length > length)
            {
                return @this.Substring(0, length);
            }

            return @this;
        }


        public string Repeat(
            int count,
            string separator = ""
        )
        {
            return string.Join(separator, Enumerable.Repeat(@this, count));
        }

        public string EncodeToBase64()
        {
            if (string.IsNullOrEmpty(@this))
            {
                return @this;
            }

            return Convert.ToBase64String(Encoding.UTF8.GetBytes(@this));
        }

        public string DecodeBase64Text()
        {
            if (string.IsNullOrEmpty(@this))
            {
                return @this;
            }

            return Encoding.UTF8.GetString(Convert.FromBase64String(@this));
        }
    }
}