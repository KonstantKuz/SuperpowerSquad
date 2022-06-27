using System;
using Survivors.Extension;
using Survivors.Units.Target;
using Survivors.Units.Weapon.Projectiles.Params;
using UnityEngine;

namespace Survivors.Units.Weapon.Projectiles
{
    public class BallLightning : Projectile
    {
        private Vector3 _targetPosition;
        [SerializeField]
        private AnimationCurve _animationCurve;
        [SerializeField]
        private float _speed;    
        [SerializeField]
        private float _timeFactor;   
        [SerializeField]
        private float _xFactor;
        [SerializeField]
        private float _timeBeforeDestroy;      
        [SerializeField]
        private float _stopDistance;    
        [SerializeField]
        private Transform _root;
        
        private float _timer;
        private float _time;
        private float _stratX;

        private bool isStop;

        private bool CanDestroy => _timer >= _timeBeforeDestroy;
        private bool IsTargetPositionReached => Vector3.Distance(transform.position.XZ(), _targetPosition.XZ()) < _stopDistance;
        public override void Launch(ITarget target, IProjectileParams projectileParams, Action<GameObject> hitCallback)
        {
            base.Launch(target, projectileParams, hitCallback);

            _targetPosition = target.Root.position;
            transform.localScale *= Params.DamageRadius;
            _stratX = _root.localPosition.x;
        }

        private void Update()
        {
            if (IsTargetPositionReached)
            {
                _timer += Time.deltaTime;
            } else {
                UpdatePosition();
            }
            if (CanDestroy) {
                Destroy();
            }
        }
        private void UpdatePosition()
        {
            _time += Time.deltaTime * _speed * _timeFactor;
            if (_time > 1) {
                _time = 0;
            }
            var newVector = transform.forward * _speed * Time.deltaTime;
            transform.position += newVector;
            _root.localPosition = new Vector3(_stratX - _animationCurve.Evaluate(_time) * _xFactor, _root.localPosition.y, _root.localPosition.z);


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
            var targetDirection = collider.transform.position - transform.position;
      


            TryHitTargetsInRadius(transform.position, 
                                  Params.DamageRadius,
                                  TargetType,
                                  null,
                                  HitCallback);
        }
    }
}