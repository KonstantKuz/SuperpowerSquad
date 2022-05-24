using System;
using Survivors.Units.Target;
using UnityEngine;

namespace Survivors.Units.Weapon.Projectiles
{
    public class CircularSaw : Projectile
    {
        [SerializeField] private float _interpolationSpeed;
        
        private Transform _rotationCenter;
        private float _currentPlaceAngle;

        public void SetRotationCenter(Transform center)
        {
            _rotationCenter = center;
        }
        
        public void SetPlaceAngle(float angle)
        {
            _currentPlaceAngle = angle;
        }
        
        public override void Launch(ITarget target, ProjectileParams projectileParams, Action<GameObject> hitCallback)
        {
            Params = projectileParams;
            HitCallback = hitCallback;
            TargetType = target.UnitType.GetTargetUnitType();
        }

        public void OnTriggerEnter(Collider collider)
        {
            if (!CanDamageTarget(collider, TargetType, out var target)) {
                return;
            }
            TryHit(collider.gameObject, transform.position, -transform.forward);
        }

        private void Update()
        {
            _currentPlaceAngle += Params.Speed;
            var targetPosition = _rotationCenter.position + 
                                         Quaternion.AngleAxis(_currentPlaceAngle, _rotationCenter.up) * _rotationCenter.forward * Params.AttackDistance;
            transform.position = Vector3.Lerp(transform.position, targetPosition, _interpolationSpeed);
        }
    }
}