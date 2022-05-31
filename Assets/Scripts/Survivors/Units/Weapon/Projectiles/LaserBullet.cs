using System;
using Survivors.Units.Target;
using UnityEngine;

namespace Survivors.Units.Weapon.Projectiles
{
    public class LaserBullet : Bullet
    {
        [SerializeField]
        private LineRenderer _lineRenderer;
        [SerializeField]
        private BoxCollider _collider;
        public override void Launch(ITarget target, ProjectileParams projectileParams, Action<GameObject> hitCallback)
        {
            base.Launch(target, projectileParams, hitCallback);
            SetWidth(projectileParams.DamageRadius);
        }

        private void SetWidth(float width)
        {
            if (width == 0) {
                return;
            }
            _lineRenderer.startWidth = width;      
            _lineRenderer.endWidth = width;
            var colliderSize = _collider.size;
            _collider.size = new Vector3(width, colliderSize.y, colliderSize.z);
        }

        public void OnTriggerEnter(Collider collider)
        {
            if (!CanDamageTarget(collider, TargetType, out var target)) {
                return;
            }
            TryHit(collider.gameObject, transform.position, -transform.forward);
        }

        protected override void TryHit(GameObject target, Vector3 hitPos, Vector3 collisionNorm)
        {
            HitCallback?.Invoke(target);
            PlayVfx(hitPos, collisionNorm);
        }
    }
}