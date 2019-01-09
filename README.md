# Useful Extension methods

Currently  wiki is under working, you could take a look into test project to get examples.

# Type Decorated

* Exception
* Expression
* ICollection
* IDicationary
* IEnumerable
* IQueryable
* Object
* String

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

