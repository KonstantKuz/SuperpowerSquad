using System;
using DG.Tweening;
using Logger.Extension;
using Survivors.Extension;
using Survivors.Location.Model;
using Survivors.Units.Component.Health;
using Survivors.Units.Player.Damageable;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Survivors.WorldEvents.Events.Lava
{
    public class Lava : WorldObject
    {
        private HittingTargetsInRadius _hittingTargets;

        private LavaEventConfig _config;

        private void Awake()
        {
            _hittingTargets = gameObject.RequireComponent<HittingTargetsInRadius>();
            transform.localScale = Vector3.one;
        }

        public void Init(LavaEventConfig config)
        {
            _config = config;

        
            var radius = Random.Range(config.Radius, config.Radius * 1.5f);
          

                    
            transform.DOScale(GetScale(radius), Random.Range(3, 5)).onComplete = () => {
                
                _hittingTargets.Init(transform.position, config.Radius, config.DamagePeriod, DoDamage);
            };

            _hittingTargets.Init(transform.position, radius, config.DamagePeriod, DoDamage); 
        }

        private Vector3 GetScale(float radius)
        {
            var scale = radius * 2;
            return new Vector3(scale, transform.localScale.y, scale);
        }

        private void DoDamage(GameObject target)
        {
            var damageable = target.RequireComponent<IDamageable>();
            damageable.TakeDamage(CalculateDamage(damageable));
            this.Logger().Trace($"Lava, damage applied, target:= {target.name}");
        }

        public void Dispose()
        {
            transform.DOScale(Vector3.one, 1f).onComplete = () => {
                
                Destroy(gameObject);
            };
            
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