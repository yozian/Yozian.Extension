using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Yozian.Extension.Dtos;

namespace Yozian.Extension;

public static class IEnumerableExtension
{
    extension<T>(IEnumerable<T> @this)
    {
        public void ForEach(
            Action<T> processor,
            bool useNewList = false
        )
        {
            // clone & could modify on origin collection
            var cloned = useNewList ? @this.ToList() : @this;

            foreach (var item in cloned)
            {
                processor(item);
            }
        }

        public void ForEach(
            Action<T, int> processor,
            bool useNewCollection = false
        )
        {
            // clone & could modify on origin collection dimension
            var collection = useNewCollection ? @this.ToList() : @this;
            var index = 0;

            foreach (var item in collection)
            {
                processor(item, index);
                index++;
            }
        }

        public async Task ForEachAsync(
            Func<T, int, CancellationToken, Task> processor,
            bool useNewCollection = false,
            CancellationToken cancellationToken = default
        )
        {
            // clone & could modify on origin collection dimension
            var collection = useNewCollection ? @this.ToList() : @this;
            var index = 0;

            foreach (var item in collection)
            {
                await processor(
                    item,
                    index,
                    cancellationToken
                );

                index++;
            }
        }

        public string FlattenToString(string separator = "")
        {
            return string.Join(separator, @this.Select(x => x.ToString()));
        }

        public string FlattenToString(
            Func<T, string> converter,
            string separator = ""
        )
        {
            return string.Join(separator, @this.Select(converter));
        }

        /// <summary>
        /// loop for items by paging
        /// </summary>
        /// <param name="limits"></param>
        /// <param name="processor"></param>
        public void ForEachPage(
            int limits,
            Action<IEnumerable<T>, int> processor
        )
        {
            var pagination = ToPagination(@this, limits);

            pagination.Pages.ForEach((page, index) =>
                {
                    // page start from 1
                    processor(page, index + 1);
                }
            );
        }

        private Pagination<T> ToPagination(
            int limits
        )
        {
            if (limits <= 0)
            {
                throw new ArgumentException("Limits should be greater than 0!");
            }

            var source = @this.ToList();

            var pageCount = 0;

            if (source.Count() % limits == 0)
            {
                pageCount = source.Count() / limits;
            }
            else
            {
                pageCount = source.Count() / limits + 1;
            }

            var pagination = new Pagination<T>
            {
                PageCount = pageCount,
                Limits = limits,
            };

            var pages = new List<IEnumerable<T>>();

            var page = 0;

            while (page < pagination.PageCount)
            {
                var processList = source
                    .Skip(limits * page)
                    .Take(limits)
                    .ToList();

                if (!processList.Any())
                {
                    break;
                }

                pages.Add(processList);

                page++;
            }

            pagination.Pages = pages;

            return pagination;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="targets"></param>
        /// <param name="comparer"></param>
        /// <param name="hashCode"></param>
        /// <returns></returns>
        public IEnumerable<T> Except(
            IEnumerable<T> targets,
            Func<T, T, bool> comparer,
            Func<T, int> hashCode = null
        )
        {
            return @this.Except(
                targets,
                new GenericComparer<T>(comparer, hashCode)
            );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool IsNullOrEmpty()
        {
            return @this == null || !@this.Any();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="batchSize">take out max amount each batch</param>
        /// <param name="consumer"></param>
        /// <returns></returns>
        public async Task<int> BatchConsumeAsync(
            int batchSize,
            Func<List<T>, CancellationToken, Task> consumer,
            CancellationToken cancellationToken = default
        )
        {
            // prevent Queue some thing like that to be used
            if (batchSize <= 0)
            {
                throw new ArgumentException("Batch size should be greater than 0!");
            }

            if (null == consumer)
            {
                throw new ArgumentNullException(nameof(consumer));
            }

            // should not be the type of IProducerConsumerCollection
            if (@this is IProducerConsumerCollection<T>)
            {
                throw new ArgumentException("The source should not be the type of IProducerConsumerCollection");
            }

            var totalCount = 0;

            using var enumerator = @this.GetEnumerator();

            while (enumerator.MoveNext())
            {
                var items = new List<T>
                {
                    enumerator.Current,
                };

                // Take out batch items to deal
                while (items.Count < batchSize && enumerator.MoveNext())
                {
                    items.Add(enumerator.Current);
                }

                // execute
                await consumer(items, cancellationToken);

                totalCount += items.Count;
            }

            return totalCount;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inner"></param>
        /// <param name="outerKeySelector"></param>
        /// <param name="innerKeySelector"></param>
        /// <param name="resultSelector"></param>
        /// <typeparam name="TInner"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <returns></returns>
        public IEnumerable<TResult> LeftOuterJoin<TInner, TKey, TResult>(
            IEnumerable<TInner> inner,
            Func<T, TKey> outerKeySelector,
            Func<TInner, TKey> innerKeySelector,
            Func<T, TInner, TResult> resultSelector
        )
        {
            return @this
                .GroupJoin(
                    inner,
                    outerKeySelector,
                    innerKeySelector,
                    (a, b) => new
                    {
                        a,
                        b,
                    }
                )
                .SelectMany(x => x.b.DefaultIfEmpty(), (x, b) => resultSelector(x.a, b));
        }
    }
}

internal class Pagination<T>
{
    public int PageCount { get; set; }

    public int Limits { get; set; }

    public IEnumerable<IEnumerable<T>> Pages { get; set; }
}
