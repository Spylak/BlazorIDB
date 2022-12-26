using BlazorIDB;
using BlazorTest.Entities;
using Microsoft.JSInterop;
using static System.Net.Mime.MediaTypeNames;

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
