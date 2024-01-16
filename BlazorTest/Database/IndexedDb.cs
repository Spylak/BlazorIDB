using BlazorIDB;
using BlazorTest.Entities;
using Microsoft.JSInterop;

namespace BlazorTest.Database
{
    public class IndexedDb : BlazorIndexedDb
    {
        public readonly IndexedDbTable<MyEntity> Entities;

        public IndexedDb(IJSRuntime jsRuntime) : base(jsRuntime)
        {
            Entities = new IndexedDbTable<MyEntity>(jsRuntime, "myentity");
        }
    }
}
