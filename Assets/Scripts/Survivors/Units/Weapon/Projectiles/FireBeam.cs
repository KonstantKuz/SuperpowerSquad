using System;
using Survivors.Units.Component.Health;
using Survivors.Units.Target;
using UnityEngine;

namespace Survivors.Units.Weapon.Projectiles
{
    public class FireBeam : Beam
    {
        [SerializeField] private DamageRangeCone _damageRangeCone;

        public override void Launch(ITarget target, ProjectileParams projectileParams, Action<GameObject> hitCallback, Transform barrel)
        {
            base.Launch(target, projectileParams, hitCallback, barrel);
            _damageRangeCone.Create(projectileParams.AttackDistance, projectileParams.DamageAngle);
        }

        protected override void TryHit(GameObject target)
        {
            TryHitTargetsInCone();
        }

        private void TryHitTargetsInCone()
        {
            var hits = Projectile.GetHits(Barrel.position, ProjectileParams.AttackDistance, TargetType);
            foreach (var hit in hits) {
                if (!IsTargetInsideCone(hit.transform.position, Barrel.position, Barrel.forward, ProjectileParams.DamageAngle)) {
                    continue;
                }
                if (hit.TryGetComponent(out IDamageable damageable)) {
                    HitCallback?.Invoke(hit.gameObject);
                }
            }
        }
        private static bool IsTargetInsideCone(Vector3 target, Vector3 coneOrigin, Vector3 coneDirection, float maxAngle)
        {
            var targetDirection = target - coneOrigin;
            var angle = Vector3.Angle(coneDirection, targetDirection);
            return angle <= maxAngle;
        }
    }
}