using Microsoft.JSInterop;

namespace BlazorIDB
{
    public class IndexedDbTable<T> : IndexedDbFunctions<T> where T : class
    {
        public IndexedDbTable(IJSRuntime jsRuntime, string TableName) : base(jsRuntime, TableName)
        {
        }
    }
}
