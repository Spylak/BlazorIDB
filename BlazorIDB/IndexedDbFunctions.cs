using Microsoft.JSInterop;

namespace BlazorIDB
{

    public class IndexedDbFunctions<T> where T : class
    {
        private readonly IJSRuntime _jsRuntime;
        private readonly string _tableName;
        const string ImportPath = "./_content/BlazorIDB/indexedDb.js";
        public IndexedDbFunctions(IJSRuntime jsRuntime, string tableName)
        {
            _jsRuntime = jsRuntime;
            _tableName = tableName;
        }

        public async Task<bool> Add(T entity)
        {
            var indexedDb = await _jsRuntime.InvokeAsync<IJSObjectReference>(
                "import", ImportPath);
            var entities = await indexedDb.InvokeAsync<List<T>>("getItem", _tableName);
            if (entities is null)
            {
                entities = new List<T>();
            }
            entity.GetType().GetProperty("Id").SetValue(entity, Guid.NewGuid());
            entities.Add(entity);
            var result = indexedDb.InvokeVoidAsync("postItem", _tableName, entities);
            return result.IsCompletedSuccessfully;
        }
        public async Task<bool> AddRange(List<T> entitiesIn)
        {
            var indexedDb = await _jsRuntime.InvokeAsync<IJSObjectReference>(
                "import", ImportPath);
            var entities = await indexedDb.InvokeAsync<List<T>>("getItem", _tableName);
            if (entities is null)
            {
                entities = new List<T>();
            }

            for (int i = 0; i < entitiesIn.Count; i++)
            {
                entitiesIn[0].GetType().GetProperty("Id").SetValue(entitiesIn[i], Guid.NewGuid());
            }
            entities.AddRange(entitiesIn);
            var result = indexedDb.InvokeVoidAsync("postItem", _tableName, entities);
            return result.IsCompletedSuccessfully;
        }

        public async Task<bool> Update(T entity)
        {
            var indexedDb = await _jsRuntime.InvokeAsync<IJSObjectReference>(
                "import", ImportPath);
            var entities = await indexedDb.InvokeAsync<List<T>>("getItem", _tableName);
            if (entities is null)
            {
                entities = new List<T>();
            }

            var item = entities.FirstOrDefault(i => i.GetType().GetProperty("Id") == entity.GetType().GetProperty("Id"));
            entities.Remove(item);
            entities.Add(entity);
            var result = indexedDb.InvokeVoidAsync("putItem", _tableName, entities);
            return result.IsCompletedSuccessfully;
        }

        public async Task<bool> UpdateRange(List<T> entitiesIn)
        {
            if (entitiesIn.Count == 0)
                return false;
            var indexedDb = await _jsRuntime.InvokeAsync<IJSObjectReference>(
                "import", ImportPath);
            var entities = await indexedDb.InvokeAsync<List<T>>("getItem", _tableName);
            if (entities is null)
            {
                entities = new List<T>();
            }

            foreach (var itm in entitiesIn)
            {
                var item = entities.FirstOrDefault(i => i.GetType().GetProperty("Id") == itm.GetType().GetProperty("Id"));
                entities.Remove(item);
                entities.Add(itm);
            }

            var result = indexedDb.InvokeVoidAsync("putItem", _tableName, entities);
            return result.IsCompletedSuccessfully;
        }

        public async Task<List<T>> GetAll()
        {
            var indexedDb = await _jsRuntime.InvokeAsync<IJSObjectReference>(
                "import", ImportPath);
            var result = await indexedDb.InvokeAsync<List<T>>("getItem", _tableName);
            if (result is null)
            {
                return new List<T>();
            }

            return result;
        }

        public async Task<bool> Remove(T entity)
        {
            var indexedDb = await _jsRuntime.InvokeAsync<IJSObjectReference>(
                "import", ImportPath);
            var entities = await indexedDb.InvokeAsync<List<T>>("getItem", _tableName);
            if (entities is null)
            {
                entities = new List<T>();
            }
            var item = entities.FirstOrDefault(i => i.GetType().GetProperty("Id") == entity.GetType().GetProperty("Id"));
            entities.Remove(item);
            var result = indexedDb.InvokeVoidAsync("putItem", _tableName, entities);
            return result.IsCompletedSuccessfully;
        }

        public async Task<bool> RemoveRange(List<T> entitiesIn)
        {
            if (entitiesIn.Count == 0)
                return false;
            var indexedDb = await _jsRuntime.InvokeAsync<IJSObjectReference>(
                "import", ImportPath);
            var entities = await indexedDb.InvokeAsync<List<T>>("getItem", _tableName);
            if (entities is null)
            {
                entities = new List<T>();
            }

            foreach (var itm in entitiesIn)
            {
                var item = entities.FirstOrDefault(i => i.GetType().GetProperty("Id") == itm.GetType().GetProperty("Id"));
                entities.Remove(item);
            }
            var result = indexedDb.InvokeVoidAsync("putItem", _tableName, entities);
            return result.IsCompletedSuccessfully;
        }

        public async Task<bool> DropTable()
        {
            var indexedDb = await _jsRuntime.InvokeAsync<IJSObjectReference>(
                "import", ImportPath);
            var result = indexedDb.InvokeAsync<bool>("removeItem", _tableName);
            return result.IsCompletedSuccessfully;
        }
    }
}
