using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Yozian.Extension;

/// <summary>
/// </summary>
public static class IProducerConsumerCollectionExtension
{
    /// <summary>
    /// </summary>
    /// <param name="this"></param>
    /// <param name="batchSize">take out max amount each batch</param>
    /// <param name="consumer"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static async Task<int> BatchConsumeAsync<T>(
        this IProducerConsumerCollection<T> @this,
        int batchSize,
        Func<IList<T>, CancellationToken, Task> consumer,
        CancellationToken cancellationToken = default
    )
    {
        if (batchSize <= 0)
        {
            throw new ArgumentException("Batch size should be greater than 0!");
        }

        if (null == consumer)
        {
            throw new ArgumentNullException(nameof(consumer));
        }

        var totalCount = 0;

        while (@this.Count > 0)
        {
            var items = new List<T>();

            // Take out batch items to deal
            while (@this.Count > 0 && items.Count < batchSize)
            {
                if (@this.TryTake(out var item))
                {
                    items.Add(item);
                }
            }

            // execute
            await consumer(items, cancellationToken);

            totalCount += items.Count;
        }

        return totalCount;
    }
}
