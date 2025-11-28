## Yozian.Extension

Yozian.Extension bundles reusable extension methods that target common .NET types (collections, LINQ, strings, diagnostics, pagination, etc.). The package targets .NET Standard 2.0 so it works with .NET Framework, .NET Core, and .NET 5+ apps.

## Install

```bash
dotnet add package Yozian.Extension
```

> See the `nuget/` folder or [NuGet.org](https://www.nuget.org/packages/Yozian.Extension) for published versions.

## Highlights

- Conditional LINQ filters with `WhereWhen` to drop `if` chains around `IQueryable` queries.
- Server-friendly pagination with async helpers and navigation metadata (`Paging<T>` and `Page<T>`).
- Productivity helpers for collections, dictionaries, producer/consumer queues, GUIDs, strings, and builders.
- Diagnostics helpers such as exception dump formatting and expression member-name extraction.
- Pure extension API surface—add the namespace once (`using Yozian.Extension;`) and keep your domain objects clean.

## Extension Catalog

| Target type | Notable members | Source / Tests |
| --- | --- | --- |
| `Exception` | `DumpDetail()` formats nested messages and filtered stack frames | `src/Yozian.Extension/ExceptionExtension.cs` · `src/Yozian.Extension.Test/ExceptionExtensionTest.cs` |
| `LambdaExpression` | `GetMemberName()` resolves the underlying member/method | `src/Yozian.Extension/ExpressionExtension.cs` · `.../ExpressionExtensionTest.cs` |
| `ICollection<T>` | `AddWhen`, `RemoveWhen`, `AddRange`, `DifferFrom()` diff reports | `src/Yozian.Extension/ICollectionExtension.cs` · `.../ICollectionExtensionTest.cs` |
| `IDictionary<TKey,TValue>` | `SafeGet()` overloads, `MergeDictionary()` | `src/Yozian.Extension/IDictionaryExtension.cs` |
| `IEnumerable<T>` | `ForEach`, `ForEachAsync`, `FlattenToString`, `BatchProcessAsync`, `LeftOuterJoin` | `src/Yozian.Extension/IEnumerableExtension.cs` · `.../IEnumerableExtensionTest.cs` |
| `IProducerConsumerCollection<T>` | `BatchConsumeAsync()` dequeues in batches with backpressure | `src/Yozian.Extension/IProducerConsumerCollectionExtension.cs` |
| `IQueryable<T>` | `WhereWhen(bool|Func<bool>)` conditional predicates | `src/Yozian.Extension/IQueryableExtension.cs` · `.../IQueryableExtensionTest.cs` |
| `object`/`T` | `SafeToString`, `ConvertAll<TTarget>()`, `ShallowClone()` | `src/Yozian.Extension/ObjectExtension.cs` |
| `string` | `ToEnum<T>()`, `LimitLength`, `Repeat`, Base64 encode/decode | `src/Yozian.Extension/StringExtension.cs` · `.../StringExtensionTest.cs` |
| `StringBuilder` | `AppendWhen`, `AppendLineWhen` overloads | `src/Yozian.Extension/StringBuilderExtension.cs` |
| `Guid` / `Guid?` | `IsEmpty`, `IsNullOrEmpty`, `Increment()` | `src/Yozian.Extension/GuidExtension.cs` |
| `Version` | `IncreaseMajor/Minor/Build` helpers | `src/Yozian.Extension/VersionExtension.cs` |
| `Enum` | `GetAttributeOf<TAttribute>()` | `src/Yozian.Extension/EnumExtension.cs` |
| `IQueryable<T>` & pagination DTOs | `ToPagination`, `ToPaginationAsync`, `ToPage` navigation metadata | `src/Yozian.Extension/Pagination/*.cs` · `.../PaginationExtensionTest.cs` |

The accompanying test project (`src/Yozian.Extension.Test`) demonstrates real-world scenarios for every extension.

## Usage Highlights

### Conditional LINQ Filters

```csharp
var books = await dbContext.Book
    .Where(x => x.Name == request.Name)
    .WhereWhen(!string.IsNullOrEmpty(request.Category), x => x.Category == request.Category)
    .WhereWhen(() => request.PublishDate.HasValue, x => x.PublishDate == request.PublishDate)
    .ToListAsync(cancellationToken);
```

`WhereWhen` keeps query composition readable—predicates run only when their boolean flag (or delegate) evaluates to `true`.

### IQueryable Pagination & Navigation

```csharp
var paging = await dbContext.Book.AsQueryable()
    .ToPaginationAsync(page: request.Page, size: request.Size, cancellationToken);

Page<Book> page = paging.ToPage(navigatorPageSize: 5);

if (page.HasNextPages)
{
    // use page.NavigationPages / NextStartPageNo for UI
}
```

- `Paging<T>` captures `TotalCount`, `PageCount`, `Size`, and the materialized page records.
- `ToPage(int navigatorPageSize)` computes navigation metadata (previous/next page groups, page links, etc.).
- Need mapping? call `paging.MapTo(dto => new BookDto(dto))` before returning to clients.

### Collection & Queue Helpers

```csharp
var diff = sourceProducts.DifferFrom(targetProducts, (a, b) => a.Id == b.Id);

await backlog.BatchProcessAsync(
    batchSize: 100,
    async (items, token) => await SendBatchAsync(items, token),
    cancellationToken);

await queue.BatchConsumeAsync(
    batchSize: 50,
    async (items, token) => await PersistAsync(items, token),
    cancellationToken);
```

- `DifferFrom` returns matches plus missing items on either side (see `Dtos/CollectionDifference.cs`).
- `BatchProcessAsync` iterates an `IEnumerable<T>` in batches without loading the whole set into memory.
- `BatchConsumeAsync` works with `IProducerConsumerCollection<T>` (e.g., `ConcurrentQueue<T>`) and drains items safely.

### Diagnostics & Conversion Helpers

- `DumpDetail()` renders nested exception messages plus per-frame file/line info with an optional filter.
- `GetMemberName()` resolves the member string from expression trees—handy for guard clauses and refactor-safe logging.
- `SafeToString()`, `ConvertAll<TTarget>()`, and `ShallowClone()` simplify object formatting and property transforms.
- `StringExtension` adds Base64 encode/decode, length limiting, `Repeat`, and safe `Enum` parsing with optional fallback.
- `Guid` and `Version` helpers cover common checks (`IsNullOrEmpty`, `IsEmpty`) and incremental versioning.

## Samples & Tests

- Explore `src/Yozian.Extension.Test` for runnable examples of every API.
- The test project is referenced throughout this README so you can jump directly to scenarios you care about.

> Contributions and new extension ideas are welcome—please file an issue or open a PR!




