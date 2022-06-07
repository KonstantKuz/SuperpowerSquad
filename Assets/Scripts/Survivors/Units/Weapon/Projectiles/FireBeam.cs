using System;
using System.Collections;
using SuperMaxim.Core.Extensions;
using Survivors.Extension;
using Survivors.Units.Component.Health;
using Survivors.Units.Target;
using Survivors.Units.Weapon.Projectiles.Params;
using UnityEngine;

namespace Survivors.Units.Weapon.Projectiles
{
    public class FireBeam : Projectile
    {
        [SerializeField] private float _flameWidth = 2;
        [SerializeField] private float _emissionCountMultiplier = 0.05f;
        [SerializeField] private float _flameLifeTimeMultiplier = 1.1f;
        [SerializeField] private float _destroyDelay = 1f;
        [SerializeField] private ParticleSystem[] _flameParticles;

        private float FlameLifeTime => _flameLifeTimeMultiplier * Params.AttackDistance / Speed;
        private float FlameThrowDuration => _flameWidth / Speed;

        private float _lifeTime;
        private float _flameStartRadius;
        private float _flameEndRadius;
        
        public override void Launch(ITarget target, IProjectileParams projectileParams, Action<GameObject> hitCallback)
        {
            base.Launch(target, projectileParams, hitCallback);
            _flameParticles.ForEach(UpdateFlameParams);
            _flameParticles.ForEach(it => it.Play());
            StartCoroutine(BurnTargets());
        }

        private void UpdateFlameParams(ParticleSystem flame)
        {
            var mainModule = flame.main;
            mainModule.startSpeed = Speed;
            mainModule.startLifetime = FlameLifeTime;
            mainModule.duration = FlameThrowDuration;
            var shapeModule = flame.shape;
            shapeModule.angle = Params.DamageAngle / 2;
            var emissionModule = flame.emission;
            emissionModule.rateOverTime = Params.DamageAngle * Params.AttackDistance * Speed * _emissionCountMultiplier;
        }

        private IEnumerator BurnTargets()
        {
            _lifeTime = 0f;
            _flameStartRadius = 0f;
            _flameEndRadius = 0f;
            var distanceTraveled = 0f;
            while (_lifeTime < FlameLifeTime)
            {
                var deltaTime = Time.deltaTime;
                var deltaDistance = deltaTime * Speed;
                _lifeTime += deltaTime;
                
                _flameEndRadius += deltaDistance;
                if (_lifeTime > FlameThrowDuration)
                {
                    _flameStartRadius += deltaDistance;
                }

                distanceTraveled += deltaDistance;
                if (distanceTraveled > _flameWidth)
                {
                    distanceTraveled = 0;
                    TryHitTargetsInFlame(_flameStartRadius, _flameEndRadius);
                    // Debug.Break();
                }
                
                yield return null;
            }
            yield return new WaitForSeconds(_destroyDelay);
            Destroy(gameObject);
        }

        private void TryHitTargetsInFlame(float flameStartRadius, float flameEndRadius)
        {
            var hits = GetHits(transform.position, flameEndRadius, TargetType);
            foreach (var hit in hits)
            {
                if (!IsTargetInsideCone(hit.transform.position, transform.position, transform.forward, Params.DamageAngle)
                || !IsTargetInsideDistanceRange(hit.transform.position, transform.position, flameStartRadius, flameEndRadius)) 
                {
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
            var angle = Vector3.Angle(coneDirection, targetDirection.XZ());
            return angle <= maxAngle;
        }

        private static bool IsTargetInsideDistanceRange(Vector3 target, Vector3 origin, float distanceMin, float distanceMax)
        {
            var distance = Vector3.Distance(origin, target);
            return distance > distanceMin && distance < distanceMax;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, _flameStartRadius);
        
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, _flameEndRadius);
        }
    }
}