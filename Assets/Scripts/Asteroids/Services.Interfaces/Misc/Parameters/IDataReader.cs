using System;

namespace Asteroids.Services
{
    public interface IDataReader<T> where T : class
    {
        T Data { get; }
        event Action<T> Changed;
    }
}