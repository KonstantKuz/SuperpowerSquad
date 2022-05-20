using System;
using Survivors.Units.Component.Health;
using Survivors.Units.Target;
using UnityEngine;

namespace Survivors.Units.Weapon.Projectiles
{
    public class Beam : MonoBehaviour
    {
        [SerializeField]
        private float _maxLifeTime;
        [Range(0f, 1f)]
        [SerializeField]
        private float _ratioHitTime;

        private ITarget _target;
        private Action<GameObject> _hitCallback;
        private ProjectileParams _projectileParams;
        private IBarrelOwner _barrel;

        private float _lifeTime;
        private bool _hit;
        private bool _initialized;
        private float HitTime => _maxLifeTime * _ratioHitTime;

        public void Launch(ITarget target, ProjectileParams projectileParams, Action<GameObject> hitCallback, IBarrelOwner barrel)
        {
            _projectileParams = projectileParams;
            _hitCallback = hitCallback;
            _barrel = barrel;
            SetTarget(target);
            _initialized = true;
        }

        private void SetTarget(ITarget target)
        {
            if (_target != null) {
                ClearTarget();
            }
            _target = target;
            _target.OnTargetInvalid += ClearTarget;
        }

        private void Update()
        {
            if (!_initialized) {
                return;
            }
            UpdateLifeTime();
            UpdatePositionAndRotation();
        }

        private void LateUpdate()
        {
            if (_barrel != null && _target != null) {
                transform.SetPositionAndRotation(_barrel.BarrelPos, 
                                                 RangedWeapon.GetShootRotation(_barrel.BarrelPos, _target.Center.position));
            }
        }

        private void UpdatePositionAndRotation()
        {
          
        }

        private void UpdateLifeTime()
        {
            _lifeTime += Time.deltaTime;
            if (_lifeTime >= HitTime && !_hit) {
                TryHit();
            }
            if (_lifeTime >= _maxLifeTime) {
                Destroy();
            }
        }

        private void TryHit()
        {
            if (_hit) {
                return;
            }
            _hit = true;
            if (_target == null) {
                return;
            }
            Hit(_target);
        }

        private void Hit(ITarget target)
        {
            var targetObj = target as MonoBehaviour;
            if (targetObj == null) {
                Debug.LogWarning("Target is not a monobehaviour");
                return;
            }
            if (targetObj.GetComponent<IDamageable>() == null) {
                return;
            }
            _hitCallback?.Invoke(targetObj.gameObject);
            Projectile.TryHitTargetsInRadius(targetObj.gameObject.transform.position, _projectileParams.DamageRadius, target.UnitType,
                                             targetObj.gameObject, _hitCallback);
        }

        private void Destroy()
        {
            gameObject.SetActive(false);
            ClearTarget();
            _hitCallback = null;
            Destroy(gameObject);
        }

        protected virtual void ClearTarget()
        {
            if (_target != null) {
                _target.OnTargetInvalid -= ClearTarget;
            }
            _target = null;
        }
    }
}