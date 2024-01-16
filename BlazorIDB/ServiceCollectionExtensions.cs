using Microsoft.Extensions.DependencyInjection;

namespace BlazorIDB;

public static class ServiceCollectionExtensions
{
    public static void AddBlazorIDB<T>(this IServiceCollection serviceCollection) where T : BlazorIndexedDb
    {
        serviceCollection.AddSingleton<T>();
    }
}