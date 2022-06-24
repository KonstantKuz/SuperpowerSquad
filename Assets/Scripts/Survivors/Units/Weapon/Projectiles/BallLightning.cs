using System;
using Survivors.Extension;
using Survivors.Units.Target;
using Survivors.Units.Weapon.Projectiles.Params;
using UnityEngine;

namespace Survivors.Units.Weapon.Projectiles
{
    public class BallLightning : Projectile
    {
        private const float STOPPING_DISTANCE = 0.2f;

        private Vector3 _targetPosition;
        [SerializeField]
        private AnimationCurve _animationCurve;
        [SerializeField]
        private float _speed;
        [SerializeField]
        private float _timeBeforeDestroy;
        
        private float _timer;
        private float _time;
        private float _stratX;

        private bool CanDestroy => _timer >= _timeBeforeDestroy;
        private bool IsTargetPositionReached => Vector3.Distance(transform.position.XZ(), _targetPosition.XZ()) < STOPPING_DISTANCE;
        public override void Launch(ITarget target, IProjectileParams projectileParams, Action<GameObject> hitCallback)
        {
            base.Launch(target, projectileParams, hitCallback);

            _targetPosition = target.Root.position;
            transform.localScale *= Params.DamageRadius;
            _stratX = transform.localPosition.x;
        }

        private void Update()
        {
            UpdatePosition();
            if (IsTargetPositionReached)
            {
                _timer += Time.deltaTime;
            }
            if (CanDestroy) {
                Destroy();
            }
        }
        private void UpdatePosition()
        {
            _time += Time.deltaTime;
            if (_time > 1) {
                _time = 0;
            }
            var newVector = transform.forward * _speed * Time.deltaTime;
            transform.position += newVector;
            transform.localPosition = new Vector3(_stratX - _animationCurve.Evaluate(_time), transform.localPosition.y, transform.localPosition.z);


        }
        private void Destroy()
        {
            HitCallback = null;
            Destroy(gameObject);
        }
        
        public void OnTriggerEnter(Collider collider)
        {
            if (!CanDamageTarget(collider, TargetType, out var target)) {
                return;
            }
            TryHitTargetsInRadius(transform.position, 
                Params.DamageRadius,
                TargetType,
                null,
                HitCallback);
        }
    }
}