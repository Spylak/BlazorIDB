using BlazorIDB;
using BlazorTest;
using BlazorTest.Database;
using BlazorTest.Services;
using BlazorTest.Services.IServices;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddMudServices();
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddBlazorIDB<IndexedDb>();
builder.Services.AddTransient<IGlobalService, GlobalService>();

await builder.Build().RunAsync();
