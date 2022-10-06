# BlazorIDB

This is a simple wrapper of the idb-keyval.js library for Blazor applications. It is designed to be used similar to Entity Framework Database.

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
builder.Services.AddSingleton<IndexedDb>();
```

### Inject the database class to any Blazor page you want to use it

```c#
@inject IndexedDb _indexedDb
```

### Use it as follows

```c#
//Get all
var allEntities = await _indexedDb.Entity.GetAll();

//GetById, the id used is a string
var getEntity = await _indexedDb.Entity.GetById(id);

//When adding either single entity or many they generate a new Guid that is then saved as string.
//Add
var entity=new MyEntity()
{
    Props...
}
await _indexedDb.Entity.Add(entity);
//AddRange
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
await _indexedDb.Entity.AddRange(entityList);

//Update
entity.Prop = SomethingNew;
await _indexedDb.Entity.Update(entity);

//UpdateRange
entityList.FirstOrDefault().Prop = SomethingNew;
await _indexedDb.Entity.UpdateRange(entityList);

//Remove
await _indexedDb.Entity.Remove(entity);

//RemoveRange
await _indexedDb.Entity.RemoveRange(entityList);

//DropTable, this deletes the key provided to the entity and its values
await _indexedDb.Entity.DropTable();
```
