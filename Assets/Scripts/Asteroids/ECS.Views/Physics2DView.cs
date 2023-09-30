using System.Collections.Generic;
using System.Linq;
using ECS;
using UnityEngine;

namespace Asteroids.ECS.Views
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Physics2DView : EntityView
    {
        public Rigidbody2D Rigidbody2D;

        public override void SetPosition(Vector2 position, float degreeAngle)
        {
            base.SetPosition(position, degreeAngle);
            Rigidbody2D.position = position;
            Rigidbody2D.rotation = degreeAngle;
        }

        public void PhysicsMovement(Vector2 position, float degreeAngle)
        {
            Rigidbody2D.MovePosition(position);
            Rigidbody2D.MoveRotation(degreeAngle);
        }

        public void TeleportMovement(Vector2 position, float degreeAngle)
        {
            SetPosition(position, degreeAngle);
        }


        #region Collision registration
        private readonly HashSet<EntityView> _registeredCollisions = new HashSet<EntityView>(3);

        private void OnTriggerEnter2D(Collider2D other)
        {
            var entityView = other.GetComponentInParent<EntityView>();
            if (entityView == null) return;

            _registeredCollisions.Add(entityView);
        }

        public EntityView[] GetCollisions()
        {
            var ret = _registeredCollisions.ToArray();
            _registeredCollisions.Clear();

            return ret;
        }
        #endregion
    }
}