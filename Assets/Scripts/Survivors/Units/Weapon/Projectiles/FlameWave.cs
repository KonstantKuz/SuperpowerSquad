using System;
using System.Linq;
using SuperMaxim.Core.Extensions;
using Survivors.Units.Target;
using Survivors.Units.Weapon.Projectiles.Params;
using UnityEngine;

namespace Survivors.Units.Weapon.Projectiles
{
    public class FlameWave : Projectile
    {
        [SerializeField] private float _destroyDelay = 1f;
        [SerializeField] private GameObject _trigger;
        [SerializeField] private GameObject _particlesRoot;
        
        [SerializeField] private ParticleSystem[] _constantSpeedParticles;
        [SerializeField] private ParticleSystem[] _randomSpeedParticles;

        private float _lifeTime;

        private float FlameLifeTime => Params.AttackDistance / Speed;
        // обнаружил, что партиклы движутся быстрее в зависимости от скейла
        // если фактическую скорость не поделить на размер, то триггер очень сильно отстает от партиклов при damage radius > 1
        private float ParticlesSpeed => Speed / Params.DamageRadius;
        
        public override void Launch(ITarget target, IProjectileParams projectileParams, Action<GameObject> hitCallback)
        {
            base.Launch(target, projectileParams, hitCallback);
            InitScale();
            InitParticles();
        }

        private void InitScale()
        {
            _trigger.transform.localScale *= Params.DamageRadius;
            _particlesRoot.transform.localScale *= Params.DamageRadius;
        }

        private void InitParticles()
        {
            _constantSpeedParticles.ForEach(SetConstantSpeed);
            _randomSpeedParticles.ForEach(SetRandomSpeed);
            var flameParticles = _constantSpeedParticles.Concat(_randomSpeedParticles);
            flameParticles.ForEach(SetLifeTime);
            flameParticles.ForEach(it => it.Play());
        }

        private void SetConstantSpeed(ParticleSystem obj)
        {
            var velocityModule = obj.velocityOverLifetime;
            velocityModule.y = ParticlesSpeed;
        }

        private void SetRandomSpeed(ParticleSystem obj)
        {
            var velocityModule = obj.velocityOverLifetime;
            velocityModule.y = new ParticleSystem.MinMaxCurve(ParticlesSpeed / 2, ParticlesSpeed);
        }
        
        private void SetLifeTime(ParticleSystem flame)
        {
            var mainModule = flame.main;
            mainModule.startLifetime = FlameLifeTime;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!CanDamageTarget(other, TargetType, out var target))
            {
                return;
            }
            HitCallback?.Invoke(other.gameObject);
        }
        
        private void Update()
        {
            _lifeTime += Time.deltaTime;
            _trigger.SetActive(_lifeTime <= FlameLifeTime);
            if (_lifeTime <= FlameLifeTime)
            {
                MoveFlameTrigger();
            }
            if (_lifeTime > FlameLifeTime + _destroyDelay)
            {
                Destroy(gameObject);
            }
        }

        private void MoveFlameTrigger()
        {
            _trigger.transform.localPosition += Vector3.forward * Time.deltaTime * Speed;
        }
    }
}
