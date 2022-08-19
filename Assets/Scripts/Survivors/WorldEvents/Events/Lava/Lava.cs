﻿using System;
using DG.Tweening;
using Logger.Extension;
using Survivors.Extension;
using Survivors.Location.Model;
using Survivors.Units.Component.Health;
using Survivors.Units.Player.Damageable;
using Survivors.WorldEvents.Events.Lava.Config;
using UnityEngine;

namespace Survivors.WorldEvents.Events.Lava
{
    public class Lava : WorldObject
    {
        private HittingTargetsInRadius _hittingTargets;

        private LavaEventConfig _config;
        
        private Tween _appearTween;
        private Tween _disappearTween;

        private void Awake()
        {
            _hittingTargets = gameObject.RequireComponent<HittingTargetsInRadius>();
            transform.localScale = Vector3.one;
        }

        public void Init(LavaEventConfig config)
        {
            DisposeTween();
            _config = config;
            var lavaRadius = _config.RandomRadius;

            _appearTween = transform.DOScale(GetScale(lavaRadius), _config.RandomAppearTime);
            _appearTween.onComplete = () => {
                _hittingTargets.Init(transform.position, lavaRadius, config.DamagePeriod, DoDamage);
                _appearTween = null;
            };
        }
        public void Dispose()
        {
            _hittingTargets.Dispose();
            _disappearTween = transform.DOScale(Vector3.one, _config.RandomDisappearTime);
            _disappearTween.onComplete = () => {
                Destroy(gameObject);
                _disappearTween = null;
            };
        }
        private void DisposeTween()
        {
            _appearTween?.Kill(true); 
            _disappearTween?.Kill(true);
        }
        private void OnDisable()
        {
            DisposeTween();
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