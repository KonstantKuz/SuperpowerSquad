using System;
using Survivors.Extension;
using Survivors.Location.Service;
using Survivors.Units.Target;
using Survivors.Units.Weapon.Projectiles;
using Survivors.Units.Weapon.Projectiles.Params;
using UnityEngine;
using UnityEngine.Assertions;
using Zenject;

namespace Survivors.Units.Weapon
{
    public class MeteorWeapon : BaseWeapon
    {
        [SerializeField] private float _startHeight;
        [SerializeField] private Meteor _meteor;
        
        [Inject]
        private WorldObjectFactory _objectFactory;

            
        public override void Fire(ITarget target, IProjectileParams projectileParams, Action<GameObject> hitCallback)
        {
            Assert.IsNotNull(projectileParams);
            var projectile = _objectFactory.CreateObject(_meteor.gameObject).RequireComponent<Meteor>();
            projectile.transform.SetPositionAndRotation(
                target.Root.position + _startHeight * Vector3.up,
                Quaternion.LookRotation(Vector3.down));

            projectile.Launch(target.UnitType, 
                projectileParams,
                _startHeight / projectileParams.Speed,
                projectileParams.Speed,
                hitCallback);
        }
    }
}