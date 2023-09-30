using System;
using Services;

namespace Asteroids.Services
{
    public interface IUpdateService : IService
    {
        event Action FixedUpdate;
        event Action Update;
        event Action LateUpdate;
    }
}