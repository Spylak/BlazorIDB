using Microsoft.JSInterop;

namespace BlazorIDB;

public class BlazorIndexedDb
{
    private readonly IJSRuntime _jsRuntime;
    const string ImportPath = "./_content/BlazorIDB/indexedDb.js";

    public BlazorIndexedDb(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
    }
    
    public async Task<ResponseIDB> ClearStore()
    {
        try
        {
            var indexedDb = await _jsRuntime
                .InvokeAsync<IJSObjectReference>("import", ImportPath);

            await indexedDb
                .InvokeAsync<bool>("clearStore");

            return new ResponseIDB(true);
        }
        catch (Exception ex)
        {
            return new ResponseIDB(false, ex.Message, ErrorCode.ExceptionError);
        }
    }
        
    public async Task<ResponseIDB<List<string>>> GetKeys()
    {
        try
        {
            var indexedDb = await _jsRuntime
                .InvokeAsync<IJSObjectReference>("import", ImportPath);

            var keys = await indexedDb
                .InvokeAsync<List<string>>("getKeys");

            return new ResponseIDB<List<string>>(keys,true);
        }
        catch (Exception ex)
        {
            return new ResponseIDB<List<string>>(null,false, ex.Message, ErrorCode.ExceptionError);
        }
    }
    public async Task<ResponseIDB> DropTable(string? tableName)
    {
        try
        {
            if (tableName is null)
                return new ResponseIDB(false, "Table name doesn't exist.", ErrorCode.TableNameDoesNotExist);
            var indexedDb = await _jsRuntime
                .InvokeAsync<IJSObjectReference>("import", ImportPath);

            await indexedDb
                .InvokeAsync<bool>("removeItem", tableName);

            return new ResponseIDB(true);
        }
        catch (Exception ex)
        {
            return new ResponseIDB(false, ex.Message, ErrorCode.ExceptionError);
        }
    }
}