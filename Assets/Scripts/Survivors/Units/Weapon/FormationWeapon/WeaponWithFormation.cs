using System;
using Survivors.Location.ObjectFactory.Factories;
using Survivors.Units.Target;
using Survivors.Units.Weapon.Projectiles;
using Survivors.Units.Weapon.Projectiles.Params;
using UnityEngine;
using Zenject;

namespace Survivors.Units.Weapon.FormationWeapon
{
    public class WeaponWithFormation : MonoBehaviour
    {
        [SerializeField] private GameObject _ammo;
        [SerializeField] private Transform _barrel;
        
        private Coroutine _attackCoroutine;
        private int _attackNumber;

        private IFireFormation _currentFormation;

        [Inject]
        protected ObjectInstancingFactory ObjectFactory;
        
        public void Fire(ProjectileFormationType formationType, ITarget target, IProjectileParams projectileParams, Action<GameObject> hitCallback)
        {
            _currentFormation = CreateFormation(formationType);
            _attackCoroutine = StartCoroutine(_currentFormation.Fire(CreateProjectile, target, projectileParams, hitCallback));
            _attackNumber++;
        }
        
        private IFireFormation CreateFormation(ProjectileFormationType attackType)
        {
            return attackType switch
            {
                ProjectileFormationType.Volley => new Volley(_barrel, 0.2f),
                ProjectileFormationType.Wave => new Wave(_barrel, _attackNumber, 3f),
                ProjectileFormationType.Wedge => new Wedge(_barrel, 3,3),
                _ => throw new ArgumentOutOfRangeException(nameof(attackType), attackType, null)
            };
        }

        private Projectile CreateProjectile()
        {
            return ObjectFactory.Create<Projectile>(_ammo.gameObject);
        }

        public void StopAttack()
        {
            if (_attackCoroutine != null)
            {
                StopCoroutine(_attackCoroutine);
                _attackCoroutine = null;
            }            
        }
    }
}