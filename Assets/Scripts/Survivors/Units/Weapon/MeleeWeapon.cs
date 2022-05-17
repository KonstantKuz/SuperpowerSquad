using System;
using Survivors.Units.Damageable;
using Survivors.Units.Target;
using Survivors.Units.Weapon.Projectile;
using UnityEngine;

namespace Survivors.Units.Weapon
{
    public class MeleeWeapon : BaseWeapon
    {
        public override void Fire(ITarget target, ProjectileParams chargeParams, Action<GameObject> hitCallback)
        {
            var targetObj = target as MonoBehaviour;
            if (targetObj == null)
            {
                Debug.LogWarning("Target is not a monobehaviour");
                return;
            }

            if (targetObj.gameObject.GetComponent<IDamageable>() == null)
            {
                Debug.LogWarning("Target has no damageable component");
                return;
            }
            hitCallback?.Invoke(targetObj.gameObject);
        }
    }
}