using System;
using System.Diagnostics.Contracts;
using System.Linq;

namespace Yozian.Extension;

public static class ObjectExtension
{
    extension(object @this)
    {
        public string SafeToString()
        {
            return @this == null ? string.Empty : @this.ToString();
        }
    }

    /// <summary>
    /// apply the expression to all TTarget properties
    /// </summary>
    extension<T>(T @this)
        where T : class
    {
        public T ConvertAll<TTarget>(Func<TTarget, TTarget> transform)
        {
            Contract.Ensures(Contract.Result<TTarget>() != null);
            var propInfo = @this.GetType()
                .GetProperties()
                .Where(x => x.PropertyType == typeof(TTarget));

            foreach (var prop in propInfo)
            {
                var currentVal = (TTarget)prop.GetValue(@this, null);
                var newVal = transform(currentVal);
                prop.SetValue(
                    @this,
                    newVal,
                    null
                );
            }

            return @this;
        }
    }

    extension<T>(T @this)
        where T : new()
    {
        public T ShallowClone()
        {
            var obj = new T();
            obj.GetType()
                .GetProperties()
                .ForEach(prop => { prop.SetValue(obj, prop.GetValue(@this)); });

            return obj;
        }
    }
}