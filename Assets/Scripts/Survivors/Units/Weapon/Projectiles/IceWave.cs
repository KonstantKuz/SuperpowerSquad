using System;
using System.Collections;
using Survivors.Units.Weapon.Projectiles.Params;
using UnityEngine;

namespace Survivors.Units.Weapon.Projectiles
{
    public class IceWave : MonoBehaviour
    {
        [SerializeField] private float _triggerScaleMultiplier;
        [SerializeField] private SphereCollider _trigger;
        [SerializeField] private Transform _particlesRoot;
        [SerializeField] private ParticleSystem[] _particles;

        private UnitType _targetType;
        private IProjectileParams _projectileParams;
        private Action<GameObject> _hitCallback;
        private float _lifeTime;
        
        private float Speed => _projectileParams.Speed;
        private float DamageRadius => _projectileParams.DamageRadius;
        
        public void Launch(UnitType targetType, Transform parent, IProjectileParams projectileParams, Action<GameObject> hitCallback)
        {
            _targetType = targetType;
            _projectileParams = projectileParams;
            _hitCallback = hitCallback;
            
            InitTransform(parent);
            InitParticles();
        }

        private void InitTransform(Transform parent)
        {
            transform.SetParent(parent);
            transform.localPosition = Vector3.zero;
            _particlesRoot.localScale = Vector3.one * DamageRadius;
            _trigger.radius = 0f;
        }

        private void InitParticles()
        {
            _lifeTime = DamageRadius / Speed;
            foreach (var particle in _particles)
            {
                var main = particle.main;
                main.startLifetime = _lifeTime;

                ScaleStartSpeed(main, 1f / _lifeTime);
            }
        }

        private void ScaleStartSpeed(ParticleSystem.MainModule main, float scaleFactor)
        {
            var startSpeed = main.startSpeed;
            if (Mathf.Abs(startSpeed.constant) < Mathf.Epsilon)
            {
                return;
            }

            startSpeed.constantMin *= scaleFactor;
            startSpeed.constantMax *= scaleFactor;
            main.startSpeed = startSpeed;
        }

        private void Update()
        {
            _lifeTime -= Time.deltaTime;
            if (_lifeTime <= 0f)
            {
                Destroy(gameObject);
                return;
            }
            ScaleUpTrigger();
        }

        private void ScaleUpTrigger()
        {
            _trigger.radius += Speed * _triggerScaleMultiplier * Time.deltaTime;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!Projectile.CanDamageTarget(other, _targetType, out var target))
            {
                return;
            }
            _hitCallback?.Invoke(other.gameObject);
        }
    }
}
