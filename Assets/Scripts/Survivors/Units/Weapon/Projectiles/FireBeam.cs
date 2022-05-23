using Survivors.Units.Component.Health;
using UnityEngine;

namespace Survivors.Units.Weapon.Projectiles
{
    public class FireBeam : Beam
    {
        [SerializeField]
        private float _fireAngle = 30;

        protected override void TryHit(GameObject target)
        {
            base.TryHit(target);
            TryHitTargetsInCone(target);
        }

        private void TryHitTargetsInCone(GameObject excludedTarget)
        {
            var hits = Projectile.GetHits(Barrel.position, ProjectileParams.AttackDistance, TargetType);
            foreach (var hit in hits) {
                if (hit.gameObject == excludedTarget) {
                    continue;
                }
                if (!IsTargetInsideCone(hit.transform.position, Barrel.position, Barrel.forward, _fireAngle, ProjectileParams.AttackDistance)) {
                    continue;
                }
                if (hit.TryGetComponent(out IDamageable damageable)) {
                    HitCallback?.Invoke(hit.gameObject);
                }
            }
        }

        private static bool IsTargetInsideCone(Vector3 target, Vector3 coneOrigin, Vector3 coneDirection, float maxAngle, float maxDistance)
        {
            var distanceToConeOrigin = (target - coneOrigin).magnitude;
            if (distanceToConeOrigin > maxDistance) {
                return false;
            }
            var targetDirection = target - coneOrigin;
            var angle = Vector3.Angle(coneDirection, targetDirection);
            return angle <= maxAngle;
        }
    }
}