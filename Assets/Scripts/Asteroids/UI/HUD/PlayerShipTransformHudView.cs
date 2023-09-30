using Asteroids.UIEntityData;
using TMPro;

namespace Asteroids.UI
{
    public class PlayerShipTransformHudView : DataView<PlayerShipData>
    {
        public TextMeshProUGUI Position;
        public TextMeshProUGUI Rotation;
        public TextMeshProUGUI Speed;

        protected override void UpdateState(PlayerShipData data)
        {
            Position.text = $"{data.Position.x:0.}x{data.Position.y:0.}";
            Rotation.text = $"{data.RotationDegreeAngle:0}\x00B0";
            Speed.text = $"{data.Speed.magnitude:0.0}m/s";
        }
    }
}