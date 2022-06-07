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
    public class FlameCharge : Projectile
    {
        private const float MAX_FLAME_ANGLE = 80f;
        private const float MIN_POSSIBLE_EMISSION_DURATION = 0.05f;
        private const float DAMAGE_DISTANCE_STEP_MULTIPLIER = 0.8f;
        
        [SerializeField] private float _initialFlameWidth = 3f;
        [SerializeField] private float _emissionCountMultiplier = 0.05f;
        [SerializeField] private float _flameLifeTimeMultiplier = 1.1f;
        [SerializeField] private float _destroyDelay = 1f;
        [SerializeField] private ParticleSystem[] _flameParticles;
        
        private float _flameDistanceMin;
        private float _flameDistanceMax;
        private float _distanceTraveled;

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
            while (lifeTime < FlameLifeTime)
            {
                lifeTime += Time.deltaTime;
                MoveFlame(lifeTime);
                TryDamageInFlame();
                yield return null;
            }
            yield return new WaitForSeconds(_destroyDelay);
            Destroy(gameObject);
        }

        private void MoveFlame(float lifeTime)
        {
            var deltaDistance = Time.deltaTime * Speed;
            _distanceTraveled += deltaDistance;

            _flameDistanceMax += deltaDistance;
            if (lifeTime > FlameThrowDuration)
            {
                _flameDistanceMin += deltaDistance;
            }
        }

        private void TryDamageInFlame()
        {
            if (_distanceTraveled >= FlameWidth * DAMAGE_DISTANCE_STEP_MULTIPLIER)
            {
                _distanceTraveled = 0;
                TryHitTargetsInFlame(_flameDistanceMin, _flameDistanceMax);
            }
        }

        private void TryHitTargetsInFlame(float flameDistanceMin, float flameDistanceMax)
        {
            var hits = GetHits(transform.position, flameDistanceMax, TargetType);
            foreach (var hit in hits)
            {
                if (!IsTargetInsideFlame(hit.transform.position, flameDistanceMin, flameDistanceMax)) 
                {
                    continue;
                }
                if (hit.TryGetComponent(out IDamageable damageable)) {
                    HitCallback?.Invoke(hit.gameObject);
                }
            }
        }

        private bool IsTargetInsideFlame(Vector3 target, float flameDistanceMin, float flameDistanceMax)
        {
            return IsInsideCone(target, transform.position, transform.forward, FlameAngle) &&
                   IsInsideDistanceRange(target, transform.position, flameDistanceMin, flameDistanceMax);
        }
        
        private static bool IsInsideCone(Vector3 target, Vector3 coneOrigin, Vector3 coneDirection, float maxAngle)
        {
            var targetDirection = target - coneOrigin;
            var angle = Vector3.Angle(coneDirection, targetDirection.XZ());
            return angle <= maxAngle;
        }

        private static bool IsInsideDistanceRange(Vector3 target, Vector3 origin, float distanceMin, float distanceMax)
        {
            var distance = Vector3.Distance(origin, target);
            return distance > distanceMin && distance < distanceMax;
        }
    }
}