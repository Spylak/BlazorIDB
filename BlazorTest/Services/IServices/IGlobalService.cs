namespace BlazorTest.Services.IServices
{
    public interface IGlobalService
    {
        Task ConsoleLog<T>(T obj) where T : class;
    }
}
