using UnityEngine;
using System;
using System.Collections;
using System.Linq;
using Survivors.Units.Component.Health;
using Survivors.Units.Target;

namespace Survivors.WorldEvents.Events.Lava
{
    public class HittingTargetsInRadius : MonoBehaviour
    {
        private Coroutine _hitCoroutine;

        public bool Enabled { get; set; } = true;

        public void Init(Vector3 hitPosition, float damageRadius, float period, Action<GameObject> hitCallback)
        {
            Dispose();
            _hitCoroutine = StartCoroutine(HitTargetsInRadius(hitPosition, damageRadius, period, hitCallback));
        }
        public void Dispose()
        {
            if (_hitCoroutine == null) {
                return;
            }
            StopCoroutine(_hitCoroutine);
            _hitCoroutine = null;
        }
        
        private IEnumerator HitTargetsInRadius(Vector3 hitPosition, float damageRadius, float period, Action<GameObject> hitCallback)
        {
            while (true) {
                yield return new WaitForSeconds(period);
                HitTargetsInRadius(hitPosition, damageRadius, hitCallback);
            }
        }

        private void HitTargetsInRadius(Vector3 hitPosition, float damageRadius, Action<GameObject> hitCallback)
        {
            if (!Enabled) {
                return;
            }
            var hits = GetHits(hitPosition, damageRadius);
            foreach (var hit in hits) {
                if (hit.TryGetComponent(out IDamageable damageable)) {
                    hitCallback?.Invoke(hit.gameObject);
                }
            }
        }

        private static Collider[] GetHits(Vector3 position, float damageRadius)
        {
            var hits = Physics.OverlapSphere(position, damageRadius);
            return hits.Where(go => {
                           var target = go.GetComponent<ITarget>();
                           return target.IsTargetValidAndAlive();
                       })
                       .ToArray();
        }
     
        private void OnDisable()
        {
            Dispose();
        }
    }
}