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
        /// apply the expression to all <Target> properties
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="@this"></param>
        /// <param name="transform"></param>
        /// <returns></returns>
        public static T ConvertAll<T, Target>(this T @this, Func<Target, Target> transform)
            where T : class
        {
            Contract.Ensures(Contract.Result<Target>() != null);
            var propinfo = @this.GetType()
               .GetProperties()
               .Where(x => x.PropertyType == typeof(Target));

            foreach (var prop in propinfo)
            {
                var currentVal = (Target) prop.GetValue(@this, null);
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
