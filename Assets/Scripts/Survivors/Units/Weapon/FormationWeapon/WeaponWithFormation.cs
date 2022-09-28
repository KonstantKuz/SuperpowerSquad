using System;
using System.Collections;
using Survivors.Location.ObjectFactory.Factories;
using Survivors.Units.Target;
using Survivors.Units.Weapon.Projectiles;
using Survivors.Units.Weapon.Projectiles.Params;
using UnityEngine;
using Zenject;

namespace Survivors.Units.Weapon.FormationWeapon
{
    public abstract class WeaponWithFormation : MonoBehaviour
    {
        [SerializeField] private ProjectileFormationType _type;
        [SerializeField] private GameObject _ammo;
        [SerializeField] private Transform _barrel;

        [Inject] private ObjectInstancingFactory _objectFactory;

        protected Transform Barrel => _barrel;
        public ProjectileFormationType FormationType => _type;

        public abstract IEnumerator Fire(ITarget target, IProjectileParams projectileParams,
            Action<GameObject> hitCallback);

        protected void LaunchProjectile(Vector3 position, 
            Quaternion rotation, 
            ITarget target,
            IProjectileParams projectileParams,
            Action<GameObject> hitCallback)
        {
            var projectile =  _objectFactory.Create<Projectile>(_ammo.gameObject);
            projectile.transform.SetPositionAndRotation(position, rotation);
            projectile.transform.localScale = Vector3.one * projectileParams.DamageRadius;
            projectile.Launch(target, projectileParams, hitCallback);
        }
    }
}