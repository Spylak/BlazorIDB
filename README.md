# BlazorIDB

This is a simple wrapper of the idb-keyval.js library for Blazor applications. It is designed to be used similar to Entity Framework Database.
You can have a look at it [here](https://spylak.github.io/BlazorIDB/).
### Create an entity Class

```c#
//A property called Id with the type of string is needed
public class MyEntity
{
    public string Id {get; set;}
    public string Property {get; set;}
    public int Property2 {get; set;}
    public List<int> Property3 {get; set;}
    public InnerClass InnerProperty {get; set;}

    public class InnerClass
    {
        public string Property {get; set;}
    }
}

```

### Create Database Class

```c#
using BlazorIDB;

public class IndexedDb
{
    public IndexedDbTable<MyEntity> Entity;
    public IndexedDb(IJSRuntime jsRuntime)
    {
        //for each entity you have to provide different key, in this case we provide <<myentity>> as key
        Entity = new IndexedDbTable<MyEntity>(jsRuntime , "myentity");
    }
}
```

### Add Database Class to Services in Program.cs

```c#
builder.Services.AddBlazorIDB<IndexedDb>();

or
    
builder.Services.AddSingleton<IndexedDb>();
```

### Inject the database class to any Blazor page you want to use it

```c#
@inject IndexedDb _indexedDb
```

### Use it as follows

```c#
//Get all
var allEntities = await _indexedDb.Entity.GetAllAsync();

//GetById, the id used is a string
var getEntity = await _indexedDb.Entity.GetByIdAsync(id);

//When adding either single entity or many they generate a new Guid that is then saved as string.
//AddAsync
var entity=new MyEntity()
{
    Props...
}
await _indexedDb.Entity.AddAsync(entity);
//AddRangeAsync
var entityList = new List<MyEntity>()
{
    new MyEntity()
        {
            Props...
        },
    new MyEntity()
        {
            Props...
        }
}
await _indexedDb.Entity.AddRangeAsync(entityList);

//UpdateAsync
entity.Prop = SomethingNew;
await _indexedDb.Entity.UpdateAsync(entity);

//UpdateRangeAsync
entityList.FirstOrDefault().Prop = SomethingNew;
await _indexedDb.Entity.UpdateRangeAsync(entityList);

//RemoveAsync
await _indexedDb.Entity.RemoveAsync(entity);

//RemoveRangeAsync
await _indexedDb.Entity.RemoveRangeAsync(entityList);

//DropTableAsync, this deletes the key provided to the entity and its values
await _indexedDb.Entity.DropTableAsync();
```
