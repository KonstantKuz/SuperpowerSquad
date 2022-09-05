using System;
using System.Collections;
using Survivors.Extension;
using Survivors.Location.ObjectFactory;
using Survivors.Location.ObjectFactory.Factories;
using Survivors.Units.Target;
using Survivors.Units.Weapon.Projectiles;
using Survivors.Units.Weapon.Projectiles.Params;
using UnityEngine;
using UnityEngine.Assertions;
using Zenject;
using Random = UnityEngine.Random;

namespace Survivors.Units.Weapon
{
    public class MeteorWeapon : BaseWeapon
    {
        [SerializeField] private float _startHeight;
        [SerializeField] private Meteor _meteor;
        [SerializeField] private float _randomDelay;
       
        [Inject]
        private ObjectInstancingFactory _objectFactory;  

            
        public override void Fire(ITarget target, IProjectileParams projectileParams, Action<GameObject> hitCallback)
        {
            Assert.IsNotNull(projectileParams);

            StartCoroutine(ShootMultipleWithDelay(
                target.Root.position, 
                target.UnitType, 
                projectileParams,
                hitCallback));
        }

        private IEnumerator ShootMultipleWithDelay(Vector3 targetPos, UnitType targetUnitType, IProjectileParams projectileParams, Action<GameObject> hitCallback)
        {
            var nextPosition = targetPos;
            var shootCount = projectileParams.Count;
            
            for (int i = 0; i < shootCount; i++)
            {
                ShootAtPosition(nextPosition, targetUnitType, projectileParams, hitCallback);

                yield return new WaitForSeconds(Random.Range(0, _randomDelay / shootCount));

                nextPosition = GetRandomTargetPosition(targetPos, projectileParams);
            }
        }

        private void ShootAtPosition(Vector3 position, UnitType targetUnitType, IProjectileParams projectileParams,
            Action<GameObject> hitCallback)
        {
            var projectile = _objectFactory.Create<Meteor>(_meteor.gameObject);
            projectile.transform.position = position + _startHeight * Vector3.up;
            
            projectile.Launch(targetUnitType,
                projectileParams,
                _startHeight / projectileParams.Speed,
                projectileParams.Speed,
                hitCallback);
        }

        private static Vector3 GetRandomTargetPosition(Vector3 targetPos, IProjectileParams projectileParams)
        {
            return targetPos + Quaternion.Euler(0, Random.Range(0.0f, 360.0f), 0) * Vector3.forward * Random.Range(1.0f, 2.0f) * projectileParams.DamageRadius;
        }
    }
}