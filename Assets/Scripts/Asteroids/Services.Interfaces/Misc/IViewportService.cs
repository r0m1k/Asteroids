using Infrastructure;
using Services;
using UnityEngine;

namespace Asteroids.Services
{
    public interface IViewportService : IService, IRequireInitialize
    {
        Rect GetViewportBounds();

        void Update();
    }
}
