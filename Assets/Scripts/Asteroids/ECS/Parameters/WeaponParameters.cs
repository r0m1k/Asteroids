using Asteroids.ECS.Entities;
using Asteroids.Infrastructure;
using ECS;
using Infrastructure;
using UnityEngine;

namespace Asteroids.ECS.Parameters
{
    [CreateAssetMenu(fileName = "Weapon Parameters", menuName = "Asteroids Game/Weapon Parameters")]
    public class WeaponParameters : ScriptableObject
    {
        public WeaponType Type;

        [TypeOf(typeof(BulletEntity))]
        public string EntityType;

        // if is ZERO so unlimited
        [Range(0, 10)]
        public int MaxBullets;
        public bool IsUnlimitedBullets => MaxBullets == 0;

        // if is ZERO no cooldown

        [Range(0, 10)]
        public float FireCooldown;
        public bool HasFireCooldown => FireCooldown > 0;

        // ZERO has meaning with limited bullets
        [Range(0, 10)]
        public float RechargeCooldown;

        public EntityView BulletPrefab;

        [Range(0, 1000)]
        public int BulletSpeed;

        [Range(0, 10)]
        public float BulletLifeTime;
        public bool BulletHasLifeTime => BulletLifeTime > 0;

        [Range(1, 10)]
        public int Damage;
    }
}
