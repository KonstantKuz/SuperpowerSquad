using System;
using Survivors.Units.Target;
using UnityEngine;

namespace Survivors.Units.Weapon.Projectiles
{
    public class CircularSaw : MonoBehaviour
    {
        private UnitType _targetType;
        private ProjectileParams _projectileParams;
        private Action<GameObject> _hitCallback;
        
        public void Init(UnitType targetType, ProjectileParams projectileParams, Action<GameObject> hitCallBack)
        {
            _targetType = targetType;
            _projectileParams = projectileParams;
            _hitCallback = hitCallBack;
        }

        public void SetLocalPlaceByAngle(float angle)
        {
            transform.localPosition = 
                Quaternion.AngleAxis(angle, transform.parent.up) * Vector3.forward * _projectileParams.AttackDistance;
        }

        public void UpdateParams(ProjectileParams projectileParams)
        {
            _projectileParams = projectileParams;
        }
        
        public void OnTriggerEnter(Collider collider)
        {
            if (!Projectile.CanDamageTarget(collider, _targetType, out var target)) {
                return;
            }
            Projectile.TryHitTargetsInRadius(transform.position, 
                _projectileParams.DamageRadius,
                _targetType,
                null,
                _hitCallback);
        }
    }
}