using System;
using System.Collections.Generic;
using Services;
using UnityEngine;

namespace Asteroids.Services
{
    public interface IReadOnlyDataService : IService
    {
        IDataReader<T> GetFirst<T>() where T : class;
        IDataReader<T> GetFirst<T>(Predicate<T> predicate) where T : class;

        IDataReader<T> GetFirstOrCreate<T>() where T : class, new();

        IEnumerable<IDataReader<T>> GetAll<T>() where T : class;

        T GetScriptable<T>() where T : ScriptableObject;
    }
}