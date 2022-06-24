using System;
using Logger.Assets.Scripts.Extension;
using Survivors.Units.Component.Health;
using Survivors.Units.Target;
using Survivors.Units.Weapon.Projectiles.Params;
using UnityEngine;

namespace Survivors.Units.Weapon
{
    public class MeleeWeapon : BaseWeapon
    {
        
        public override void Fire(ITarget target, IProjectileParams chargeParams, Action<GameObject> hitCallback)
        {
            var targetObj = target as MonoBehaviour;
            if (targetObj == null)
            {
                this.Logger().Warn("Target is not a monobehaviour");
                return;
            }

            if (targetObj.GetComponent<IDamageable>() == null)
            {
                this.Logger().Warn("Target has no damageable component");
                return;
            }
            hitCallback?.Invoke(targetObj.gameObject);
        }
    }
}