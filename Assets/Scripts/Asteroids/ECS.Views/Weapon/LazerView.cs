using ECS;
using UnityEngine;

namespace Asteroids.ECS.Views
{
    public class LazerView : EntityView
    {
        public SpriteRenderer BeamSprite;

        public void SetBeamLength(float length)
        {
            if (!BeamSprite) return;

            var size = BeamSprite.size;
            size.y = length;
            BeamSprite.size = size;

            var localPosition = BeamSprite.transform.localPosition;
            localPosition.y = length / 2f;
            BeamSprite.transform.localPosition = localPosition;
        }
    }
}