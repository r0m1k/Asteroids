using Infrastructure;

namespace Asteroids.Services
{
    // ToDo: add data tag to separate same Type from instanced and collection...or just use different types
    public interface IDataService : IReadOnlyDataService, IRequireCleanUp
    {
        IDataWriter<T> Add<T>() where T : class, new();
        IDataWriter<T> Add<T>(T data) where T : class;

        new IDataWriter<T> GetFirst<T>() where T : class;
        new IDataWriter<T> GetFirstOrCreate<T>() where T : class, new();

        void RemoveAll<T>() where T : class;
        void RemoveAll();
    }
}