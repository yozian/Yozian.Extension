﻿using System;
using System.Linq;
using System.Text;

namespace Yozian.Extension;

public static class StringExtension
{
    public static T ToEnum<T>(this string @this, bool enableFailOverWithDefaultValue = true)
    {
        try
        {
            return (T)Enum.Parse(typeof(T), @this, true);
        }
        catch (Exception)
        {
            if (!enableFailOverWithDefaultValue)
            {
                throw;
            }

            return default(T);
        }
    }

    public static string LimitLength(this string @this, int length)
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


    public static string Repeat(
        this string @this,
        int count,
        string separator = ""
    )
    {
        return string.Join(separator, Enumerable.Repeat(@this, count));
    }

    public static string EncodeToBase64(this string @this)
    {
        if (string.IsNullOrEmpty(@this))
        {
            return @this;
        }

        return Convert.ToBase64String(Encoding.UTF8.GetBytes(@this));
    }

    public static string DecodeBase64Text(this string @this)
    {
        if (string.IsNullOrEmpty(@this))
        {
            return @this;
        }

        return Encoding.UTF8.GetString(Convert.FromBase64String(@this));
    }
}
