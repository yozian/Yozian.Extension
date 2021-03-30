using System;
using System.Collections.Generic;

namespace Yozian.Extension.CollectionDto
{
    /// <summary>
    ///  Note that Object HashCode is ignored here to be compared!
    /// </summary>
    public class GenericComparer<T> : IEqualityComparer<T>
    {
        private readonly Func<T, T, bool> compareMethod;
        private readonly Func<T, int> hashCodeMethod = (x)=> 0;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="compareMethod"></param>
        /// <param name="hashCodeMethod"></param>
        public GenericComparer(
            Func<T, T, bool> compareMethod,
            Func<T, int> hashCodeMethod = null)
        {
            this.compareMethod = compareMethod;
            if (hashCodeMethod != null) {
                this.hashCodeMethod = hashCodeMethod;
            }
        }

        public bool Equals(T x, T y)
        {
            return this.compareMethod(x, y);
        }

        public virtual int GetHashCode(T obj)
        {
            return this.hashCodeMethod(obj);
        }
    }
}
