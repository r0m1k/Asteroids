using Services;
using UnityEngine;

namespace Asteroids.Services
{
    public interface IAsteroidsRulesService : IService
    {
        bool InsideViewport(Vector2 point);

        float GetMinLengthToCrossViewport();
        Vector2 GetRandomSafePoint(Vector2 respectPoint, float safeRadius);

        Vector2 GetPointWithRespectToViewportTeleportation(Vector2 point);
        Vector2 GetFakeClosesPositionWithTeleportationRespect(Vector2 position, Vector2 targetPosition);
    }
}