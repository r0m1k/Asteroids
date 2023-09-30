namespace Asteroids.Services
{
    public interface IDataWriter<T> : IDataReader<T> where T : class
    {
        void Notify();
    }
}