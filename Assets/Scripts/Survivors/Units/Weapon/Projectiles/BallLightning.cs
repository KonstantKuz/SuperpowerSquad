using System;
using Survivors.Extension;
using Survivors.Location.Service;
using Survivors.Units.Target;
using Survivors.Units.Weapon.Projectiles.Params;
using UniRx;
using UnityEngine;
using Zenject;

namespace Survivors.Units.Weapon.Projectiles
{
    public class BallLightning : Projectile
    {
        [SerializeField] private AnimationCurve _animationCurve;

        [SerializeField] private float _curveTimeFactor;
        [SerializeField] private float _curveWidthFactor;
        [SerializeField] private float _destroyDelay;
        [SerializeField] private float _meshScaleFactor;

        [SerializeField] private Transform _rootContainer;
        [SerializeField] private Transform _meshContainer;

        [SerializeField] private float _lightningDuration;
        [SerializeField] private float _hitTimeout;

        [Inject]
        private WorldObjectFactory _objectFactory;
        
        private Vector3 _targetPosition;
        private float _curveTime; 
        private float _lifeTimer;      
        private float _stoppedTime;

        private LightningBoltGenerator _lightningBoltGenerator;
        private CompositeDisposable _disposable = new CompositeDisposable();
        
        private bool CanDestroy => _lifeTimer >= _stoppedTime + _destroyDelay;

        public override void Launch(ITarget target, IProjectileParams projectileParams, Action<GameObject> hitCallback)
        {
            base.Launch(target, projectileParams, hitCallback);
            _targetPosition = target.Root.position;
            _lightningBoltGenerator = gameObject.RequireComponent<LightningBoltGenerator>();
            _stoppedTime = (_targetPosition - transform.position).magnitude / Speed;

            SetMeshScale();
            TryHit();
            Observable.Interval(TimeSpan.FromSeconds(_hitTimeout)).Subscribe(it => { TryHit(); }).AddTo(_disposable);
        }

        private void SetMeshScale()
        {
            var meshScale = Params.DamageRadius * _meshScaleFactor;
            _meshContainer.localScale = Vector3.one * meshScale;
        }

        private void TryHit()
        {
            var hits = GetHits(transform.position, Params.DamageRadius, TargetType);
            foreach (var hit in hits) {
                if (!CanDamageTarget(hit, TargetType, out var target)) {
                    continue;
                }
                _lightningBoltGenerator.Hit(_objectFactory, _rootContainer, target.Center, _lightningDuration);
                HitCallback?.Invoke(hit.gameObject);
            }
        }
        
        private void Update()
        {
            _lifeTimer += Time.deltaTime;
            if (_lifeTimer < _stoppedTime) {
                UpdatePosition();
            }
            if (CanDestroy) {
                Destroy(gameObject);
            } 
        }

        private void UpdatePosition()
        {
            transform.position += transform.forward * Speed * Time.deltaTime;
            UpdateCurvePosition();
        }

        private void UpdateCurvePosition()
        {
            _curveTime += Time.deltaTime * Speed * _curveTimeFactor;
            _curveTime = Mathf.Repeat(_curveTime, 1);
            var localPosition = _rootContainer.localPosition;
            _rootContainer.localPosition = new Vector3(_animationCurve.Evaluate(_curveTime) * _curveWidthFactor, localPosition.y, localPosition.z);
        }

        private void OnDestroy()
        {
            HitCallback = null;
            _disposable?.Dispose();
            _disposable = null;
        }
    }
}