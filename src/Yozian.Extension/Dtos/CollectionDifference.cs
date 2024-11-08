using System.Collections.Generic;

namespace Yozian.Extension.Dtos;

/// <summary>
/// 
/// </summary>
/// <summary>
/// </summary>
/// <typeparam name="T"></typeparam>
public class CollectionDifference<T>
{
    /// <summary>
    /// Both exists
    /// </summary>
    public List<Difference> MatchedItems { get; }

    /// <summary>
    /// Only existing in the targets but missing in the source
    /// </summary>
    public List<T> SourceMissingItems { get; }

    /// <summary>
    /// Only Existing in the source but missing in the target
    /// </summary>
    public List<T> TargetMissingItems { get; }

    public CollectionDifference(
        List<Difference> matchedItems,
        List<T> sourceMissingItems,
        List<T> targetMissingItems
    )
    {
        this.MatchedItems = matchedItems;
        this.SourceMissingItems = sourceMissingItems;
        this.TargetMissingItems = targetMissingItems;
    }

    public class Difference
    {
        /// <summary>
        /// Source (Left)
        /// </summary>
        public T Source { get; }

        /// <summary>
        /// Target (Right)
        /// </summary>
        public T Target { get; }

        public Difference(T source, T target)
        {
            this.Source = source;
            this.Target = target;
        }
    }
}
