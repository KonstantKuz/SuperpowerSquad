using System;
using System.Linq;
using Survivors.Units.Component.Health;
using Survivors.Units.Target;
using UnityEngine;
using UnityEngine.Assertions;

namespace Survivors.Units.Weapon.Projectiles
{
    public abstract class Projectile : MonoBehaviour
    {
        protected Action<GameObject> HitCallback;
        protected UnitType TargetType;   
        protected ProjectileParams Params;
        protected float Speed => Params.Speed;
  
        
        public virtual void Launch(ITarget target, ProjectileParams projectileParams, Action<GameObject> hitCallback)
        {
            Assert.IsNotNull(target);
            HitCallback = hitCallback;
            TargetType = target.UnitType;
            Params = projectileParams;
        }
        private void OnCollisionEnter(Collision other)
        {
            var colliderTarget = other.collider.GetComponent<ITarget>();
            if (colliderTarget == null) {
                return;
            }
            if (TargetType != colliderTarget.UnitType) {
                return;
            }
            if (!other.collider.TryGetComponent(out IDamageable damageable)) {
                return;
            }
            var contact = other.GetContact(0);
            TryHit(other.gameObject, contact.point, contact.normal);
        }

        protected virtual void TryHit(GameObject target, Vector3 hitPos, Vector3 collisionNorm)
        {
            HitCallback?.Invoke(target);
            TryHitTargetsInRadius(target);
        }

        private void TryHitTargetsInRadius(GameObject excludedTarget)
        {
            var hits = GetHits(Params.DamageRadius, TargetType);
            foreach (var hit in hits) {
                if (hit.gameObject == excludedTarget) {
                    continue;
                }
                if (hit.TryGetComponent(out IDamageable damageable)) {
                    HitCallback?.Invoke(hit.gameObject);
                }
            }
        }
        private Collider[] GetHits(float damageRadius, UnitType targetType)
        {
            var hits = Physics.OverlapSphere(transform.position, damageRadius);
            return hits.Where(go => go.GetComponent<ITarget>() != null && go.GetComponent<ITarget>().IsAlive
                                    && go.GetComponent<ITarget>().UnitType == targetType).ToArray();
        }
        

    }
}