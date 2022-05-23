using System;
using System.Collections;
using ModestTree;
using Survivors.Units.Component.Health;
using Survivors.Units.Target;
using UnityEngine;

namespace Survivors.Units.Weapon.Projectiles
{
    public class Beam : MonoBehaviour
    {
        [SerializeField]
        private float _maxLifeTime;
        [Range(0f, 1f)]
        [SerializeField]
        private float _ratioHitTime;

        protected ITarget Target;
        protected UnitType TargetType;
        protected Action<GameObject> HitCallback;
        protected ProjectileParams ProjectileParams;
        protected Transform Barrel;
        
        private bool _hit;
        private float HitTime => _maxLifeTime * _ratioHitTime;

        public void Launch(ITarget target, ProjectileParams projectileParams, Action<GameObject> hitCallback, Transform barrel)
        {
            Assert.IsNotNull(target);
            ProjectileParams = projectileParams;
            HitCallback = hitCallback;
            Barrel = barrel;
            SetTarget(target); 
            StartCoroutine(UpdateLifeTime());
        }

        private void SetTarget(ITarget target)
        {
            if (Target != null) {
                ClearTarget();
            }
            Target = target;
            TargetType = target.UnitType;
            Target.OnTargetInvalid += ClearTarget;
        }
        private IEnumerator UpdateLifeTime()
        {
            yield return new WaitForSeconds(HitTime);
            TryHitTarget();
            yield return new WaitForSeconds(Math.Abs(_maxLifeTime - HitTime));
            Destroy();
        }

        private void TryHitTarget()
        {
            if (_hit) {
                return;
            }
            _hit = true;
            if (Target == null) {
                return;
            }
            Hit(Target);
        }

        private void Hit(ITarget target)
        {
            var targetObj = target as MonoBehaviour;
            if (targetObj == null) {
                Debug.LogWarning("Target is not a monobehaviour");
                return;
            }
            if (targetObj.GetComponent<IDamageable>() == null) {
                return;
            }
            TryHit(targetObj.gameObject);
        }
        protected virtual void TryHit(GameObject target)
        {
            HitCallback?.Invoke(target);
        }
        private void Destroy()
        {
            gameObject.SetActive(false);
            ClearTarget();
            HitCallback = null;
            Destroy(gameObject);
        }
        private void ClearTarget()
        {
            if (Target != null) {
                Target.OnTargetInvalid -= ClearTarget;
            }
            Target = null;
        }
    }
}