using System;
using System.Collections.Generic;

namespace Yozian.Extension.CollectionDto
{
    /// <summary>
    ///
    /// </summary>
    public class GenericComparer<T> : IEqualityComparer<T>
        where T : new()
    {
        private readonly Func<T, T, bool> compareMethod;

        public GenericComparer(Func<T, T, bool> compareMethod)
        {
            this.compareMethod = compareMethod;
        }

        public bool Equals(T x, T y)
        {
            return this.compareMethod(x, y);
        }

        public virtual int GetHashCode(T obj)
        {
            return obj.GetHashCode();
        }
    }
}
