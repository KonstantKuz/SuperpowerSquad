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

        [SerializeField] private float _queueSubInterval = 0.2f;
        [SerializeField] private float _circleRadius = 3f;
        [SerializeField] private float _arrowWidth = 2f;
        [SerializeField] private float _arrowLength = 3f;
        [SerializeField] private float _arrowSpread = 2f;
        
        private Coroutine _attackCoroutine;
        private int _attackNumber;
        private IFireFormation _currentFormation;

        [Inject]
        protected ObjectInstancingFactory ObjectFactory;
        
        public void Fire(ProjectileFormationType formationType, ITarget target, IProjectileParams projectileParams, Action<GameObject> hitCallback)
        {
            _currentFormation = CreateFormation(formationType);
            _attackCoroutine = StartCoroutine(_currentFormation.Fire(target, projectileParams, hitCallback));
            _attackNumber++;
        }
        
        private IFireFormation CreateFormation(ProjectileFormationType attackType)
        {
            return attackType switch
            {
                ProjectileFormationType.Queue => new QueueFire(CreateProjectile, _barrel, _queueSubInterval),
                ProjectileFormationType.Circle => new CircleFire(CreateProjectile, _barrel, _attackNumber, _circleRadius),
                ProjectileFormationType.Arrow => new ArrowFire(CreateProjectile, _barrel, _arrowWidth, _arrowLength, _arrowSpread),
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