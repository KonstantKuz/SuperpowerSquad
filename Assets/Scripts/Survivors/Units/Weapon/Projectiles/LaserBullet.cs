using UnityEngine;

namespace Survivors.Units.Weapon.Projectiles
{
    public class LaserBullet : Bullet
    {
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