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
        [SerializeField] private float _stopDistance;
        [SerializeField] private float _meshScaleFactor;

        [SerializeField] private Transform _rootContainer;
        [SerializeField] private Transform _meshContainer;

        [SerializeField] private float _lightningDuration;
        [SerializeField] private float _hitTimeout;

        [Inject]
        private WorldObjectFactory _objectFactory;

        private float _destroyTimer;
        private float _curveTime;
        private Vector3 _targetPosition;
        private LightningBoltGenerator _lightningBoltGenerator;
        private CompositeDisposable _disposable = new CompositeDisposable();
        private bool CanDestroy => _destroyTimer >= _destroyDelay;
        private bool IsTargetPositionReached => Vector3.Distance(transform.position.XZ(), _targetPosition.XZ()) < _stopDistance;

        public override void Launch(ITarget target, IProjectileParams projectileParams, Action<GameObject> hitCallback)
        {
            base.Launch(target, projectileParams, hitCallback);
            _targetPosition = target.Root.position;
            _lightningBoltGenerator = gameObject.RequireComponent<LightningBoltGenerator>();
            SetDamageRadius();
            SetMeshScale();
            Observable.Interval(TimeSpan.FromSeconds(_hitTimeout)).Subscribe(it => { TryHit(); }).AddTo(_disposable);
        }

        private void SetDamageRadius()
        {
            _rootContainer.gameObject.RequireComponent<SphereCollider>().radius = Params.DamageRadius;
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
                    return;
                }
                _lightningBoltGenerator.Hit(_objectFactory, _rootContainer, target.Center, _lightningDuration);
                HitCallback?.Invoke(hit.gameObject);
            }
        }

        private void Update()
        {
            if (!IsTargetPositionReached) {
                UpdatePosition();
            } else {
                _destroyTimer += Time.deltaTime;
            }
            if (CanDestroy) {
                Destroy();
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
            _curveTime = Mathf.Clamp01(_curveTime);
            var localPosition = _rootContainer.localPosition;
            _rootContainer.localPosition = new Vector3(_animationCurve.Evaluate(_curveTime) * _curveWidthFactor, localPosition.y, localPosition.z);
        }

        private void Destroy()
        {
            HitCallback = null;
            Destroy(gameObject);
            _disposable?.Dispose();
            _disposable = null;
        }
    }
}