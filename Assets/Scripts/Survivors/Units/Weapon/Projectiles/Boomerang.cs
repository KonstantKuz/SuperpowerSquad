using System;
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
        private Vector3 _initialTargetPosition;
        private Transform _returnPoint;
        private float _startTime;
        
        private float LifeTime => Time.time - _startTime;
        private bool IsTargetPositionReached => Vector3.Distance(transform.position, TargetPosition) < STOPPING_DISTANCE;
        private Vector3 TargetPosition => GetCurrentState() == BoomerangState.ReturnBack ? _returnPoint.position : _initialTargetPosition;

        public void Launch(Transform returnPoint, ITarget target, IProjectileParams projectileParams, Action<GameObject> hitCallback, Action<Boomerang> destroyCallBack)
        {
            base.Launch(target, projectileParams, hitCallback);

            _initialTargetPosition = transform.position + transform.forward * Params.AttackDistance;
            transform.localScale *= Params.DamageRadius;
            
            _destroyCallback = destroyCallBack;
            _returnPoint = returnPoint;
            _startTime = Time.time;
        }

        private void Update()
        {
            if (GetCurrentState() != BoomerangState.Stop)
            {
                UpdatePosition();
            }
            if (GetCurrentState() == BoomerangState.ReturnBack && IsTargetPositionReached)
            {
                Destroy();
            }
        }

        private BoomerangState GetCurrentState()
        {
            var moveToTargetTime = Params.AttackDistance / Speed;
            var returnBackTime = moveToTargetTime + _returnDelay;
            if (LifeTime < moveToTargetTime)
            {
                return BoomerangState.MoveToTarget;
            }
            return LifeTime < returnBackTime ? BoomerangState.Stop : BoomerangState.ReturnBack;
        }

        private void UpdatePosition()
        {
            var moveDirection = TargetPosition - transform.position;
            transform.rotation = Quaternion.LookRotation(moveDirection.XZ());
            transform.position += transform.forward * Speed * Time.deltaTime;
        }
        
        private void Destroy()
        {
            _destroyCallback?.Invoke(this);
            HitCallback = null;
            Destroy(gameObject);
        }
    }
}