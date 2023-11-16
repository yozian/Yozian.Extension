using System;
using System.Diagnostics.Contracts;
using System.Linq;

namespace Yozian.Extension
{
    public static class ObjectExtension
    {
        public static string SafeToString(this object @this)
        {
            return @this == null ? string.Empty : @this.ToString();
        }

        /// <summary>
        /// apply the expression to all TTarget properties
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="@this"></param>
        /// <param name="transform"></param>
        /// <returns></returns>
        public static T ConvertAll<T, TTarget>(this T @this, Func<TTarget, TTarget> transform)
            where T : class
        {
            Contract.Ensures(Contract.Result<TTarget>() != null);
            var propInfo = @this.GetType()
               .GetProperties()
               .Where(x => x.PropertyType == typeof(TTarget));

            foreach (var prop in propInfo)
            {
                var currentVal = (TTarget) prop.GetValue(@this, null);
                var newVal = transform(currentVal);
                prop.SetValue(@this, newVal, null);
            }

            return @this;
        }

        public static T ShallowClone<T>(this T @this)
            where T : new()
        {
            var obj = new T();
            obj.GetType()
               .GetProperties()
               .ForEach(prop => { prop.SetValue(obj, prop.GetValue(@this)); });

            return obj;
        }
    }
}
