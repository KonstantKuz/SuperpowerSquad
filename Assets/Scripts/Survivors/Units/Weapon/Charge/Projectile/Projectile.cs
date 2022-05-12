using System;
using Survivors.Units.Target;
using UnityEngine;
using UnityEngine.Assertions;

namespace Survivors.Units.Weapon.Charge.Projectile
{
    public abstract class Projectile : MonoBehaviour
    {
        protected Action<GameObject> HitCallback;
        protected UnitType TargetType;
        
        public virtual void Launch(ITarget target, Action<GameObject> hitCallback)
        {
            Assert.IsNotNull(target);
            TargetType = target.UnitType;     
            HitCallback = hitCallback;
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

        protected abstract void TryHit(GameObject target, Vector3 hitPos, Vector3 collisionNorm);

    }
}