using System;
using System.Collections;
using Survivors.Extension;
using Survivors.Units.Target;
using Survivors.Units.Weapon.Projectiles.Params;
using UnityEngine;

namespace Survivors.Units.Weapon.Projectiles
{
    public enum BoomerangState
    {
        MoveToTarget,
        Stop,
        ReturnBack
    }
    
    public class Boomerang : Projectile
    {
        private const float STOPPING_DISTANCE = 0.5f;
        [SerializeField] private float _returnDelay;

        private Action<Boomerang> _destroyCallback;
        private Vector3 _targetPosition;
        private Transform _returnPoint;
        private BoomerangState _state;

        private bool IsTargetPositionReached => Vector3.Distance(transform.position, _targetPosition) < STOPPING_DISTANCE;
        public void Launch(Transform returnPoint, ITarget target, IProjectileParams projectileParams, Action<GameObject> hitCallback, Action<Boomerang> destroyCallBack)
        {
            base.Launch(target, projectileParams, hitCallback);

            _targetPosition = transform.position + transform.forward * Params.AttackDistance;
            transform.localScale *= Params.DamageRadius;
            
            _destroyCallback = destroyCallBack;
            _returnPoint = returnPoint;
            
            StartCoroutine(UpdateState());
        }

        private IEnumerator UpdateState()
        {
            _state = BoomerangState.MoveToTarget;
            yield return new WaitForSeconds(Params.AttackDistance / Speed);
            _state = BoomerangState.Stop;
            yield return new WaitForSeconds(_returnDelay);
            _state = BoomerangState.ReturnBack;
        }

        private void Update()
        {
            if (_state != BoomerangState.Stop)
            {
                UpdatePosition();
            }

            if (_state == BoomerangState.ReturnBack && IsTargetPositionReached)
            {
                Destroy();
            }
        }

        private void UpdatePosition()
        {
            if (_state == BoomerangState.ReturnBack)
            {
                _targetPosition = _returnPoint.position;
            }
            
            var moveDirection = _targetPosition - transform.position;
            transform.rotation = Quaternion.LookRotation(moveDirection.XZ());
            transform.position += transform.forward * Speed * Time.deltaTime;
        }
        
        private void Destroy()
        {
            _destroyCallback?.Invoke(this);
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