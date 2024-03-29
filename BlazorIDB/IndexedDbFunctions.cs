﻿using Microsoft.JSInterop;

namespace BlazorIDB
{

    public class IndexedDbFunctions<T> where T : class
    {
        private readonly IJSRuntime _jsRuntime;
        private readonly string _tableName;
        private IJSObjectReference? IndexedDb { get; set; }
        public IndexedDbFunctions(IJSRuntime jsRuntime, string tableName)
        {
            _jsRuntime = jsRuntime;
            _tableName = tableName;
        }

        public async Task<ResponseIDB> AddAsync(T? entity)
        {
            try
            {
                if (entity is null)
                    return new ResponseIDB(false, "Nothing to add",ErrorCode.NullInput);

                if (entity.GetType().GetProperty("Id") is null)
                    return new ResponseIDB(false, "Entity type has not Id property.",ErrorCode.MissingIdProperty);

                IndexedDb ??= await _jsRuntime
                    .InvokeAsync<IJSObjectReference>("import", Constants.ImportPath);

                var entities = await IndexedDb
                    .InvokeAsync<List<T>?>("getItem", _tableName) ?? new List<T>();

                var id = entity.GetType().GetProperty("Id")?.GetValue(entity);

                if(string.IsNullOrWhiteSpace((string?)id))
                    entity.GetType().GetProperty("Id")?.SetValue(entity, Guid.NewGuid().ToString());

                entities.Add(entity);

                await IndexedDb
                    .InvokeVoidAsync("postItem", _tableName, entities);
                
                return new ResponseIDB(true);
            }
            catch (Exception ex)
            {
                return new ResponseIDB(false, ex.Message, ErrorCode.ExceptionError);
            }
        }
        public async Task<ResponseIDB> AddRangeAsync(IEnumerable<T>? entitiesIn)
        {
            try
            {
                if (entitiesIn is null)
                    return new ResponseIDB(false, "Entities are null", ErrorCode.NullInput);

                if (entitiesIn.Any())
                    return new ResponseIDB(false, "Nothing to add", ErrorCode.NullInput);

                if (entitiesIn.First().GetType().GetProperty("Id") is null)
                    return new ResponseIDB(false, "Entity type has not Id property.", ErrorCode.MissingIdProperty);

                IndexedDb ??= await _jsRuntime
                    .InvokeAsync<IJSObjectReference>("import", Constants.ImportPath);

                var entities = await IndexedDb
                    .InvokeAsync<List<T>?>("getItem", _tableName) ?? new List<T>();

                foreach (var item in entitiesIn)
                {
                    var id = item.GetType().GetProperty("Id")?.GetValue(item);

                    if (string.IsNullOrWhiteSpace((string?)id))
                        item.GetType().GetProperty("Id")?.SetValue(item, Guid.NewGuid().ToString());
                }

                entities.AddRange(entitiesIn);

                await IndexedDb
                    .InvokeVoidAsync("postItem", _tableName, entities);

                return new ResponseIDB(true);
            }
            catch (Exception ex)
            {
                return new ResponseIDB(false, ex.Message, ErrorCode.ExceptionError);
            }
        }
        public async Task<ResponseIDB> AddRangeAsync(ICollection<T>? entitiesIn)
        {
            try
            {
                if (entitiesIn is null)
                    return new ResponseIDB(false, "Entities are null", ErrorCode.NullInput);

                if (entitiesIn.Count.Equals(0))
                    return new ResponseIDB(false, "Nothing to add", ErrorCode.NullInput);

                if (entitiesIn.First().GetType().GetProperty("Id") is null)
                    return new ResponseIDB(false, "Entity type has not Id property.", ErrorCode.MissingIdProperty);

                IndexedDb ??= await _jsRuntime
                    .InvokeAsync<IJSObjectReference>("import", Constants.ImportPath);

                var entities = await IndexedDb
                    .InvokeAsync<List<T>?>("getItem", _tableName) ?? new List<T>();

                foreach (var item in entitiesIn)
                {
                    var id = item.GetType().GetProperty("Id")?.GetValue(item);

                    if (string.IsNullOrWhiteSpace((string?)id))
                        item.GetType().GetProperty("Id")?.SetValue(item, Guid.NewGuid().ToString());
                }

                entities.AddRange(entitiesIn);

                await IndexedDb
                    .InvokeVoidAsync("postItem", _tableName, entities);

                return new ResponseIDB(true);
            }
            catch (Exception ex)
            {
                return new ResponseIDB(false, ex.Message, ErrorCode.ExceptionError);
            }
        }

        public async Task<ResponseIDB> UpdateAsync(T? entity)
        {
            try
            {
                if (entity is null)
                    return new ResponseIDB(false, "Entity is null.", ErrorCode.NullInput);

                if (entity.GetType().GetProperty("Id") is null)
                    return new ResponseIDB(false, "Entity type has not Id property.", ErrorCode.MissingIdProperty);

                IndexedDb ??= await _jsRuntime
                    .InvokeAsync<IJSObjectReference>("import", Constants.ImportPath);

                var entities = await IndexedDb
                    .InvokeAsync<List<T>?>("getItem", _tableName);

                if (entities is null)
                    return new ResponseIDB(false, "There are no entities in the table.", ErrorCode.EntityNotFound);

                var item = entities
                    .FirstOrDefault(i => object
                    .Equals(i.GetType().GetProperty("Id")!.GetValue(i),
                    entity.GetType().GetProperty("Id")!.GetValue(entity)));

                if (item is null)
                    return new ResponseIDB(false, "No entity to update in table.", ErrorCode.EntityNotFound);

                entities.Remove(item);

                entities.Add(entity);

                await IndexedDb
                    .InvokeVoidAsync("putItem", _tableName, entities);

                return new ResponseIDB(true);
            }
            catch (Exception ex)
            {
                return new ResponseIDB(false, ex.Message, ErrorCode.ExceptionError);
            }
        }

        public async Task<ResponseIDB> UpdateRangeAsync(ICollection<T>? entitiesIn)
        {
            try
            {
                if (entitiesIn is null)
                    return new ResponseIDB(false, "Entities are null.", ErrorCode.NullInput);

                if (entitiesIn.Count.Equals(0))
                    return new ResponseIDB(false, "Nothing to update.", ErrorCode.NullInput);

                if (entitiesIn.First().GetType().GetProperty("Id") is null)
                    return new ResponseIDB(false, "Entity type has not Id property.", ErrorCode.MissingIdProperty);

                IndexedDb ??= await _jsRuntime
                    .InvokeAsync<IJSObjectReference>("import", Constants.ImportPath);

                var entities = await IndexedDb
                    .InvokeAsync<List<T>?>("getItem", _tableName);

                if (entities is null)
                    return new ResponseIDB(false, "There are no entities in the table.", ErrorCode.EntityNotFound);

                foreach (var itm in entitiesIn)
                {
                    var item = entities.FirstOrDefault(i => object
                    .Equals(i.GetType().GetProperty("Id")!.GetValue(i),
                    itm.GetType().GetProperty("Id")!.GetValue(itm)));

                    if (item is not null)
                    {
                        entities.Remove(item);
                        entities.Add(itm);
                    }
                }

                await IndexedDb
                    .InvokeVoidAsync("putItem", _tableName, entities);

                return new ResponseIDB(true);
            }
            catch (Exception ex)
            {
                return new ResponseIDB(false, ex.Message, ErrorCode.ExceptionError);
            }
        }
        public async Task<ResponseIDB> UpdateRangeAsync(IEnumerable<T>? entitiesIn)
        {
            try
            {
                if (entitiesIn is null)
                    return new ResponseIDB(false, "Entities are null.", ErrorCode.NullInput);

                if (!entitiesIn.Any())
                    return new ResponseIDB(false, "Nothing to update.", ErrorCode.NullInput);

                if (entitiesIn.First().GetType().GetProperty("Id") is null)
                    return new ResponseIDB(false, "Entity type has not Id property.", ErrorCode.MissingIdProperty);

                IndexedDb ??= await _jsRuntime
                    .InvokeAsync<IJSObjectReference>("import", Constants.ImportPath);

                var entities = await IndexedDb
                    .InvokeAsync<List<T>?>("getItem", _tableName);

                if (entities is null)
                    return new ResponseIDB(false, "There are no entities in the table.", ErrorCode.EntityNotFound);

                foreach (var itm in entitiesIn)
                {
                    var item = entities.FirstOrDefault(i => object
                    .Equals(i.GetType().GetProperty("Id")!.GetValue(i),
                    itm.GetType().GetProperty("Id")!.GetValue(itm)));

                    if (item is not null)
                    {
                        entities.Remove(item);
                        entities.Add(itm);
                    }
                }

                await IndexedDb
                    .InvokeVoidAsync("putItem", _tableName, entities);

                return new ResponseIDB(true);
            }
            catch (Exception ex)
            {
                return new ResponseIDB(false, ex.Message, ErrorCode.ExceptionError);
            }
        }

        public async Task<ResponseIDB<IEnumerable<T>>> GetAllAsync()
        {
            try
            {
                IndexedDb ??= await _jsRuntime
                    .InvokeAsync<IJSObjectReference>("import", Constants.ImportPath);

                var entities = await IndexedDb
                    .InvokeAsync<List<T>?>("getItem", _tableName);

                if (entities is null)
                    return new ResponseIDB<IEnumerable<T>>(null, false, "There are no entities in the table.", ErrorCode.EntityNotFound);

                return new ResponseIDB<IEnumerable<T>>(entities, true);
            }
            catch (Exception ex)
            {
                return new ResponseIDB<IEnumerable<T>>(null, false, ex.Message, ErrorCode.ExceptionError);
            }
        }

        public async Task<ResponseIDB<T>> GetByIdAsync(string? id)
        {
            try
            {
                if (id is null)
                    return new ResponseIDB<T>(null, false, "Id is null", ErrorCode.NullInput);

                IndexedDb ??= await _jsRuntime
                    .InvokeAsync<IJSObjectReference>("import", Constants.ImportPath);

                var entities = await IndexedDb
                    .InvokeAsync<List<T>?>("getItem", _tableName);

                if (entities is null)
                    return new ResponseIDB<T>(null, false, "There are no entities in the table.", ErrorCode.EntityNotFound);

                var item = entities
                    .FirstOrDefault(i => object
                        .Equals(i.GetType().GetProperty("Id")?.GetValue(i), id));

                if (item is null)
                    return new ResponseIDB<T>(null, false, "No entity with that id.", ErrorCode.EntityNotFound);

                return new ResponseIDB<T>(item, true);
            }
            catch (Exception ex)
            {
                return new ResponseIDB<T>(null,false, ex.Message, ErrorCode.ExceptionError);
            }
        }

        public async Task<ResponseIDB> RemoveAsync(T? entity)
        {
            try
            {
                if (entity is null)
                    return new ResponseIDB(false, "Entity is null", ErrorCode.NullInput);

                if (entity.GetType().GetProperty("Id") is null)
                    return new ResponseIDB(false, "Entity type has not Id property.", ErrorCode.MissingIdProperty);

                IndexedDb ??= await _jsRuntime
                    .InvokeAsync<IJSObjectReference>("import", Constants.ImportPath);

                var entities = await IndexedDb
                    .InvokeAsync<List<T>?>("getItem", _tableName);

                if (entities is null)
                    return new ResponseIDB(false, "There are no entities in the table.", ErrorCode.EntityNotFound);

                var item = entities
                    .FirstOrDefault(i => object.Equals(i.GetType().GetProperty("Id")?.GetValue(i),
                    entity.GetType().GetProperty("Id")!.GetValue(entity)));

                if (item is null)
                    return new ResponseIDB(false, "No entity to remove in table.", ErrorCode.EntityNotFound);

                entities.Remove(item);

                await IndexedDb
                    .InvokeVoidAsync("putItem", _tableName, entities);
                
                return new ResponseIDB(true);
            }
            catch (Exception ex)
            {
                return new ResponseIDB(false, ex.Message, ErrorCode.ExceptionError);
            }
        }

        public async Task<ResponseIDB> RemoveRangeAsync(ICollection<T>? entitiesIn)
        {
            try
            {
                if (entitiesIn is null)
                    return new ResponseIDB(false, "Entities are null", ErrorCode.NullInput);

                if (entitiesIn.Count.Equals(0))
                    return new ResponseIDB(false, "Nothing to remove", ErrorCode.NullInput);

                if (entitiesIn.First().GetType().GetProperty("Id") is null)
                    return new ResponseIDB(false, "Entity type has not Id property.", ErrorCode.MissingIdProperty);

                IndexedDb ??= await _jsRuntime
                    .InvokeAsync<IJSObjectReference>("import", Constants.ImportPath);

                var entities = await IndexedDb
                    .InvokeAsync<List<T>?>("getItem", _tableName);

                if (entities is null)
                    return new ResponseIDB(false, "There are no entities in the table.", ErrorCode.EntityNotFound);

                foreach (var itm in entitiesIn)
                {
                    var item = entities
                        .FirstOrDefault(i => object
                        .Equals(i.GetType().GetProperty("Id")?.GetValue(i),
                        itm.GetType().GetProperty("Id")?.GetValue(itm)));

                    if (item is not null)
                    {
                        entities.Remove(item);
                    }
                }

                await IndexedDb
                    .InvokeVoidAsync("putItem", _tableName, entities);

                return new ResponseIDB(true);
            }
            catch (Exception ex)
            {
                return new ResponseIDB(false, ex.Message, ErrorCode.ExceptionError);
            }
        }

        public async Task<ResponseIDB> RemoveRangeAsync(IEnumerable<T>? entitiesIn)
        {
            try
            {
                if (entitiesIn is null)
                    return new ResponseIDB(false, "Entities are null", ErrorCode.NullInput);

                if (!entitiesIn.Any())
                    return new ResponseIDB(false, "Nothing to remove", ErrorCode.NullInput);

                if (entitiesIn.First().GetType().GetProperty("Id") is null)
                    return new ResponseIDB(false, "Entity type has not Id property.", ErrorCode.MissingIdProperty);

                IndexedDb ??= await _jsRuntime
                    .InvokeAsync<IJSObjectReference>("import", Constants.ImportPath);

                var entities = await IndexedDb
                    .InvokeAsync<List<T>?>("getItem", _tableName);

                if (entities is null)
                    return new ResponseIDB(false, "There are no entities in the table.", ErrorCode.EntityNotFound);

                foreach (var itm in entitiesIn)
                {
                    var item = entities
                        .FirstOrDefault(i => object
                        .Equals(i.GetType().GetProperty("Id")?.GetValue(i),
                        itm.GetType().GetProperty("Id")?.GetValue(itm)));

                    if (item is not null)
                    {
                        entities.Remove(item);
                    }
                }

                await IndexedDb
                    .InvokeVoidAsync("putItem", _tableName, entities);

                return new ResponseIDB(true);
            }
            catch (Exception ex)
            {
                return new ResponseIDB(false, ex.Message, ErrorCode.ExceptionError);
            }
        }

        public async Task<ResponseIDB> DropTableAsync()
        {
            try
            {
                IndexedDb ??= await _jsRuntime
                    .InvokeAsync<IJSObjectReference>("import", Constants.ImportPath);

                await IndexedDb
                    .InvokeAsync<bool>("removeItem", _tableName);

                return new ResponseIDB(true);
            }
            catch (Exception ex)
            {
                return new ResponseIDB(false, ex.Message, ErrorCode.ExceptionError);
            }
        }
        private async Task<ResponseIDB<List<string>>> GetKeysAsync()
        {
            try
            {
                IndexedDb ??= await _jsRuntime
                    .InvokeAsync<IJSObjectReference>("import", Constants.ImportPath);

                var keys = await IndexedDb
                    .InvokeAsync<List<string>>("getKeys");

                return new ResponseIDB<List<string>>(keys,true);
            }
            catch (Exception ex)
            {
                return new ResponseIDB<List<string>>(null,false, ex.Message, ErrorCode.ExceptionError);
            }
        }
    }
}
