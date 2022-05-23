using Survivors.Units.Component.Health;
using Survivors.Units.Target;
using UnityEngine;

namespace Survivors.Units.Weapon.Projectiles
{
    public class LaserBullet : Bullet
    {
        public void OnTriggerEnter(Collider other)
        {
            var colliderTarget = other.GetComponent<ITarget>();
            if (colliderTarget == null) {
                return;
            }
            if (TargetType != colliderTarget.UnitType) {
                return;
            }
            if (!other.TryGetComponent(out IDamageable damageable)) {
                return;
            }
            TryHit(other.gameObject, transform.position, -transform.forward);
        }
        
        protected override void TryHit(GameObject target, Vector3 hitPos, Vector3 collisionNorm)
        {
            HitCallback?.Invoke(target);
            PlayVfx(hitPos, collisionNorm);
        }
    }
}