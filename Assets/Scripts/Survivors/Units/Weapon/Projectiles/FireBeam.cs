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
        private const float MAX_FLAME_ANGLE = 80f;
        private const float MIN_POSSIBLE_EMISSION_DURATION = 0.05f;
        private const float DAMAGE_DISTANCE_STEP_MULTIPLIER = 0.8f;
        
        [SerializeField] private float _initialFlameWidth = 3f;
        [SerializeField] private float _emissionCountMultiplier = 0.05f;
        [SerializeField] private float _flameLifeTimeMultiplier = 1.1f;
        [SerializeField] private float _destroyDelay = 1f;
        [SerializeField] private ParticleSystem[] _flameParticles;

        private float FlameThrowDuration =>  Mathf.Max(MIN_POSSIBLE_EMISSION_DURATION, _initialFlameWidth / Speed);
        private float FlameLifeTime =>_flameLifeTimeMultiplier * Params.AttackDistance / Speed;
        private float FlameWidth => FlameThrowDuration * Speed;
        private float FlameAngle => Mathf.Clamp(Params.DamageAngle / 2, 0, MAX_FLAME_ANGLE);
        
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
            mainModule.duration = FlameThrowDuration;
            mainModule.startLifetime = FlameLifeTime;
            var shapeModule = flame.shape;
            shapeModule.angle = FlameAngle;
            var emissionModule = flame.emission;
            emissionModule.rateOverTime = FlameAngle * Params.AttackDistance * Speed * _emissionCountMultiplier;
        }

        private IEnumerator BurnTargets()
        {
            var lifeTime = 0f;
            var flameStartRadius = 0f;
            var flameEndRadius = 0f;
            var distanceTraveled = 0f;
            while (lifeTime < FlameLifeTime)
            {
                var deltaTime = Time.deltaTime;
                var deltaDistance = deltaTime * Speed;
                lifeTime += deltaTime;
                
                flameEndRadius += deltaDistance;
                if (lifeTime > FlameThrowDuration)
                {
                    flameStartRadius += deltaDistance;
                }

                distanceTraveled += deltaDistance;
                if (distanceTraveled >= FlameWidth * DAMAGE_DISTANCE_STEP_MULTIPLIER)
                {
                    distanceTraveled = 0;
                    TryHitTargetsInFlame(flameStartRadius, flameEndRadius);
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
                if (!IsTargetInsideFlame(hit.transform.position, flameStartRadius, flameEndRadius)) 
                {
                    continue;
                }
                if (hit.TryGetComponent(out IDamageable damageable)) {
                    HitCallback?.Invoke(hit.gameObject);
                }
            }
        }

        private bool IsTargetInsideFlame(Vector3 target, float flameStartRadius, float flameEndRadius)
        {
            return IsTargetInsideCone(target, transform.position, transform.forward, FlameAngle) &&
                   IsTargetInsideDistanceRange(target, transform.position, flameStartRadius, flameEndRadius);
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
    }
}