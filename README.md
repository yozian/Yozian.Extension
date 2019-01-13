# Useful Extension methods

You could take a look into test project to get examples with following list!

# Extension methods of Type

* [ExceptionExtension](https://github.com/yozian/Yozian.Extension/blob/master/src/Yozian.Extension.Test//ExceptionExtensionTest.cs)

    >DumpDetail

* [ExpressionExtension](https://github.com/yozian/Yozian.Extension/blob/master/src/Yozian.Extension.Test//ExpressionExtensionTest.cs)

    >GetMemberName

* [ICollectionExtension](https://github.com/yozian/Yozian.Extension/blob/master/src/Yozian.Extension.Test//ICollectionExtensionTest.cs)

    >AddWhen

    >RemoveWhen

* [IDictionaryExtension](https://github.com/yozian/Yozian.Extension/blob/master/src/Yozian.Extension.Test//IDictionaryExtensionTest.cs)

    >SafeGet

* [IEnumerableExtension](https://github.com/yozian/Yozian.Extension/blob/master/src/Yozian.Extension.Test//IEnumerableExtensionTest.cs)

    >ForEach

    >FlattenToString

    >DistinctBy


* [IQueryableExtension](https://github.com/yozian/Yozian.Extension/blob/master/src/Yozian.Extension.Test//IQueryableExtensionTest.cs)

    >WhereWhen

* [ObjectExtension](https://github.com/yozian/Yozian.Extension/blob/master/src/Yozian.Extension.Test//ObjectExtensionTest.cs)

    >SafeToString

    >ConvertAll

    >ShallowClone

* [StringExtension](https://github.com/yozian/Yozian.Extension/blob/master/src/Yozian.Extension.Test//StringExtensionTest.cs)

    >ToEnum

    >LimitLength

    >Repeate

* [PaginationExtension](https://github.com/yozian/Yozian.Extension/blob/master/src/Yozian.Extension.Test//PaginationExtensionTest.cs)

    >ToPagination

    >ToPaginationAsync

# Take IQueryable Extension method as example


### conditional query where closure


The example shows the extension method `WhereWhen` eliminate if statement block.

The constrain applied only when the condition matched, so that you could achieve dynamic query easily!

```csharp

var books = await this.dbContext.Book
                .Where(x => x.Name.Equals(request.Name))
                .WhereWhen(
                    !string.IsNullOrEmpty(request.Category),
                    x => x.Category.Equals(request.Category)
                )
                .WhereWhen(
                    request.PublishDate.HasValue,
                    x => x.PublishDate.Equals(request.PublishDate)
                )
                .ToListAsync();

```


for old school way, would have lots of if statement block

```csharp

var query = this.dbContext.Book
                .Where(x => x.Name.Equals(request.Name));

if(!string.IsNullOrEmpty(request.Category)){
    query = query.Where(x => x.Category.Equals(request.Category))
}

if(request.PublishDate.HasValue){
    query = query.Where(x => x.PublishDate.Equals(request.PublishDate))
}

var books = await query.ToListAsync();


```


### feel free for pull request

