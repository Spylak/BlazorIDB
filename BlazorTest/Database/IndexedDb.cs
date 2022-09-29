using BlazorIDB;
using BlazorTest.Entities;
using Microsoft.JSInterop;
using static System.Net.Mime.MediaTypeNames;

namespace BlazorTest.Database
{
    public class IndexedDb
    {
        public readonly IndexedDbTable<MyEntity> Entities;

        public IndexedDb(IJSRuntime jsRuntime)
        {
            Entities = new IndexedDbTable<MyEntity>(jsRuntime, "myentity");
        }
    }
}
