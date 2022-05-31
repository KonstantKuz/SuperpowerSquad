using System;
using Survivors.Units.Target;
using Survivors.Units.Weapon.Projectiles;
using UnityEngine;

namespace Survivors.Units.Weapon
{
    public class BeamWeaponWithRangeCone : BeamWeapon
    {
        [SerializeField] private DamageRangeCone _damageRangeCone;

        public override void Fire(ITarget target, ProjectileParams projectileParams, Action<GameObject> hitCallback)
        {
            base.Fire(target, projectileParams, hitCallback);
            _damageRangeCone.Build(projectileParams.AttackDistance, projectileParams.DamageAngle);
        }
    }
}