namespace Services
{
    public interface IServiceProvider
    {
        T Get<T>() where T : class, IService;
    }
}