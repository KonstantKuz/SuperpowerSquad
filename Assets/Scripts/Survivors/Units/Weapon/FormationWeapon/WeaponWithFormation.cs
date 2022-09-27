using System;
using System.Collections;
using System.Collections.Generic;
using Feofun.Util.SerializableDictionary;
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

        protected Projectile CreateProjectile()
        {
            return _objectFactory.Create<Projectile>(_ammo.gameObject);
        }
    }
}