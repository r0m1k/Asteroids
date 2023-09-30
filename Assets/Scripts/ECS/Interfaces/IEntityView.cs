using UnityEngine;

namespace ECS
{
    public interface IEntityView
    {
        long EntityId { get; }
        GameObject GameObject { get; }

        void SetPosition(Vector2 position, float degreeAngle);
    }
}