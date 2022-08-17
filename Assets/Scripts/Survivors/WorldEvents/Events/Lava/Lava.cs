using System;
using Logger.Extension;
using Survivors.Extension;
using Survivors.Location.Model;
using Survivors.Units.Component.Health;
using Survivors.Units.Player.Damageable;
using UnityEngine;

namespace Survivors.WorldEvents.Events.Lava
{
    public class Lava : WorldObject
    {
        private HittingTargetsInRadius _hittingTargets;

        private LavaEventConfig _config;

        private void Awake()
        {
            _hittingTargets = gameObject.RequireComponent<HittingTargetsInRadius>();
        }

        public void Init(LavaEventConfig config)
        {
            _config = config;
            SetRadius(config.Radius);
            _hittingTargets.Init(transform.position, config.Radius, config.DamagePeriod, DoDamage); 
        }

        private void SetRadius(float radius)
        {
            var scale = radius * 2;
            transform.localScale = new Vector3(scale, transform.localScale.y, scale);
        }

        private void DoDamage(GameObject target)
        {
            var damageable = target.RequireComponent<IDamageable>();
            damageable.TakeDamage(CalculateDamage(damageable));
            this.Logger().Trace($"Lava, damage applied, target:= {target.name}");
        }

        public void Dispose()
        {
            Destroy(gameObject);
        }

        private float CalculateDamage(IDamageable target)
        {
            return target switch {
                    DamageableChild damageableChild => CalculateDamage(damageableChild.ParentDamageable),
                    Health health => (health.MaxValue.Value * _config.DamagePercent) / 100,
                    _ => throw new ArgumentException("IDamageable must be health")
            };
        }

    }
}