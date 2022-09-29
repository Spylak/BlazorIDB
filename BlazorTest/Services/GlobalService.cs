using BlazorTest.Services.IServices;
using Microsoft.JSInterop;

namespace BlazorTest.Services
{
    public class GlobalService : IGlobalService
    {
        private readonly IJSRuntime _jsRuntime;
        public GlobalService(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        public async Task ConsoleLog<T>(T obj) where T : class
        {
           await _jsRuntime.InvokeVoidAsync("GlobalFunctions.Log", obj);
        }
    }
}
