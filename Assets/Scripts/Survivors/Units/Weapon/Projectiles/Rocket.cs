using System;
using Survivors.Location.ObjectFactory;
using Survivors.Location.ObjectFactory.Factories;
using Survivors.Units.Target;
using Survivors.Units.Weapon.Projectiles.Params;
using UnityEngine;
using UnityEngine.Assertions;
using Zenject;

namespace Survivors.Units.Weapon.Projectiles
{
    public sealed class Rocket : Projectile
    {
        [SerializeField] private bool _followTarget;

        [SerializeField] private float _rotationSpeed;

        [SerializeField] private float _maxLifeTime;

        [SerializeField] private Explosion _explosion;

        [SerializeField] private float _detonationDistance;

        [SerializeField] private float _initialCourseTime;

        private Vector3 _lastTargetPos;
        
        [Inject(Id = ObjectFactoryType.Instancing)] 
        private IObjectFactory _objectFactory;  

        private ITarget _target;

        private float TimeLeft { get; set; }

        private float LifeTime => _maxLifeTime - TimeLeft;

        private void Update()
        {
            UpdateTargetPosition();
            UpdatePosition();

            TimeLeft -= Time.deltaTime;
            if (TimeLeft <= 0 || Vector3.Distance(transform.position, _lastTargetPos) < _detonationDistance)
                Explode(transform.position);
        }

        public override void Launch(ITarget target, IProjectileParams projectileParams, Action<GameObject> hitCallback)
        {
            base.Launch(target, projectileParams, hitCallback);
            SetTarget(target);
            TimeLeft = _maxLifeTime;
        }

        private void SetTarget(ITarget target)
        {
            Assert.IsNull(_target, "we are currently supporting only one call to SetTarget on launch");
            Assert.IsNotNull(target);
            if (!_followTarget)
            {
                _lastTargetPos = target.Center.position;
                return;
            }

            _target = target;
            _target.OnTargetInvalid += ClearTarget;
        }

        private void UpdateTargetPosition()
        {
            if (!_followTarget) return;
            if (!_target.IsTargetValidAndAlive()) return;
            _lastTargetPos = _target.Center.position;
        }

        private void UpdatePosition()
        {
            transform.position += transform.forward * Speed * Time.deltaTime;

            if (LifeTime >= _initialCourseTime && _followTarget)
            {
                var lookRotation = Quaternion.LookRotation(_lastTargetPos - transform.position);
                transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, Time.deltaTime * _rotationSpeed);
            }
        }

        protected override void TryHit(GameObject target, Vector3 hitPos, Vector3 collisionNorm)
        {
            Explode(hitPos);
        }

        private void Explode(Vector3 pos)
        {
            Explosion.Create(_objectFactory, _explosion, pos, Params.DamageRadius, TargetType, HitCallback);
            Destroy();
        }

        private void Destroy()
        {
            ClearTarget();
            HitCallback = null;
            Destroy(gameObject);
        }

        private void ClearTarget()
        {
            if (_target != null) _target.OnTargetInvalid -= ClearTarget;
            _target = null;
        }
    }
}