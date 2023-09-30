using UnityEngine;

namespace ECS
{
    public class EntityView : MonoBehaviour, IEntityView, IEntitySetId
    {
        public long EntityId { get; private set; }
        public GameObject GameObject => this ? gameObject : null;

        public void SetId(long entityId)
        {
            EntityId = entityId;
        }

        public virtual void SetPosition(Vector2 position, float degreeAngle)
        {
            transform.position = position;
            transform.rotation = Quaternion.Euler(0, 0, degreeAngle);
        }
    }
}