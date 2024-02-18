using Microsoft.JSInterop;

namespace BlazorIDB;

public class BlazorIndexedDb
{
    private readonly IJSRuntime _jsRuntime;
    private IJSObjectReference? IndexedDb { get; set; }
    public BlazorIndexedDb(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
    }
    
    public async Task<ResponseIDB> ClearStoreAsync()
    {
        try
        {
            IndexedDb ??= await _jsRuntime
                .InvokeAsync<IJSObjectReference>("import", Constants.ImportPath);
            
            await IndexedDb
                .InvokeAsync<bool>("clearStore");

            return new ResponseIDB(true);
        }
        catch (Exception ex)
        {
            return new ResponseIDB(false, ex.Message, ErrorCode.ExceptionError);
        }
    }
        
    public async Task<ResponseIDB<List<string>>> GetKeysAsync()
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
    public async Task<ResponseIDB> DropTable(string? tableName)
    {
        try
        {
            if (tableName is null)
                return new ResponseIDB(false, "Table name doesn't exist.", ErrorCode.TableNameDoesNotExist);
            IndexedDb ??= await _jsRuntime
                .InvokeAsync<IJSObjectReference>("import", Constants.ImportPath);

            await IndexedDb
                .InvokeAsync<bool>("removeItem", tableName);

            return new ResponseIDB(true);
        }
        catch (Exception ex)
        {
            return new ResponseIDB(false, ex.Message, ErrorCode.ExceptionError);
        }
    }
}