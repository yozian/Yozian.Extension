using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Yozian.Extension.Test;

[TestFixture]
public class IProducerConsumerCollectionExtensionTest
{
    [Test]
    public async Task Test_ConsumeAllItemsAsync()
    {
        var seedCount = 10;
        var batchSize = 3;

        var queue = new ConcurrentQueue<int>();

        Enumerable.Range(1, seedCount).ToList().ForEach(queue.Enqueue);

        var total = await queue.BatchConsumeAsync(
            batchSize,
            async (items, cancellationToken) =>
            {
                if (items.Count != batchSize)
                {
                    // remains
                    Assert.AreEqual(seedCount % batchSize, items.Count);
                }
                else
                {
                    Assert.AreEqual(batchSize, items.Count);
                }

                await Task.CompletedTask;
            }
        );

        // empty
        Assert.AreEqual(0, queue.Count);

        // consumed
        Assert.AreEqual(seedCount, total);
    }

    [Test]
    public void Test_BatchConsumeAsync_InvalidBatchSize()
    {
        var queue = new ConcurrentQueue<int>();

        Assert.ThrowsAsync<ArgumentException>(async () =>
            await queue.BatchConsumeAsync(0, (items, token) => Task.CompletedTask)
        );
    }

    [Test]
    public void Test_BatchConsumeAsync_NullConsumer()
    {
        var queue = new ConcurrentQueue<int>();

        Assert.ThrowsAsync<ArgumentNullException>(async () =>
            await queue.BatchConsumeAsync(1, null)
        );
    }
}
