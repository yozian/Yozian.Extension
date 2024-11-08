using System;
using System.Collections.Generic;

namespace Yozian.Extension.Dtos
{
    /// <summary>
    ///  Note that Object HashCode is ignored here to be compared!
    /// </summary>
    public class GenericComparer<T> : IEqualityComparer<T>
    {
        private readonly Func<T, T, bool> compare;
        private readonly Func<T, int> hashCode = x => 0;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="compare">identify x and y</param>
        /// <param name="hashCode">return the object's hashCode default = 0 (ignore hash comparison)</param>
        public GenericComparer(
            Func<T, T, bool> compare,
            Func<T, int> hashCode = null
        )
        {
            this.compare = compare;

            if (hashCode != null)
            {
                this.hashCode = hashCode;
            }
        }

        public bool Equals(T x, T y)
        {
            return this.compare(x, y);
        }

        public virtual int GetHashCode(T obj)
        {
            return this.hashCode(obj);
        }
    }
}
