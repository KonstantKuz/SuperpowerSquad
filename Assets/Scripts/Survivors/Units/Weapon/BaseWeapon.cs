using System;
using JetBrains.Annotations;
using Survivors.Units.Target;
using Survivors.Units.Weapon.Projectiles;
using UnityEngine;

namespace Survivors.Units.Weapon
{
    public abstract class BaseWeapon : MonoBehaviour
    {
        [SerializeField]
        protected Transform Barrel;

        protected Vector3 BarrelPos; //Seems that in some cases unity cannot correctly take position inside animation event

        public abstract void Fire(ITarget target, [CanBeNull] ProjectileParams projectileParams, Action<GameObject> hitCallback);

        private void LateUpdate()
        {
            BarrelPos = Barrel.position;
        }
    }
}