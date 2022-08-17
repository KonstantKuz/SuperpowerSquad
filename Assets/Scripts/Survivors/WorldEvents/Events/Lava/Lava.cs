using Logger.Extension;
using Survivors.Extension;
using Survivors.Units.Component.Health;
using UnityEngine;

namespace Survivors.WorldEvents.Events.Lava
{
    public class Lava : MonoBehaviour
    {
        private HittingTargetsInRadius _hittingTargets;

        private void Awake()
        {
            _hittingTargets = gameObject.RequireComponent<HittingTargetsInRadius>();
        }

        private void OnEnable()
        {
            _hittingTargets.Init(transform.position, transform.localScale.x/2, 1, DoDamage); 
        }
        
        private void DoDamage(GameObject target)
        {
            var damageable = target.RequireComponent<IDamageable>();
            damageable.TakeDamage(30);
            this.Logger().Trace($"Lava, damage applied, target:= {target.name}");
        }

    }
}