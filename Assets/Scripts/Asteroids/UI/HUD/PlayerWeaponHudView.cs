using Asteroids.Infrastructure;
using Asteroids.Services;
using Asteroids.UIEntityData;
using TMPro;

namespace Asteroids.UI
{
    public class PlayerWeaponHudView : PlayerReadyDataView<PlayerWeaponData>
    {
        public TextMeshProUGUI Count;
        public TextMeshProUGUI Cooldown;

        protected override IDataReader<PlayerWeaponData> FindSuitableData()
        {
            return _dataService.GetFirst<PlayerWeaponData>(data => data.Type == WeaponType.Laser);
        }
        
        protected override void UpdateState(PlayerWeaponData data)
        {
            if (data == null)
            {
                Count.text = Cooldown.text = "-";
                return;
            }

            Count.text = data.Bullets >= 0 ? data.Bullets.ToString() : "-";
            Cooldown.text = data.RechargeCooldown > float.Epsilon ? $"{data.RechargeCooldown:0.0}s" : "-";
        }
    }
}