using System;
using DG.Tweening;
using Feofun.Components;
using Survivors.Squad.Service;
using UniRx;
using UnityEngine;
using Zenject;

namespace Survivors.Camera
{
    public class CameraFollowBehavior : MonoBehaviour, IInitializable<Squad.Squad>
    {
        [SerializeField] 
        private float _initialDistance;
        [SerializeField] 
        private float _distanceIncreaseStep;

        [SerializeField] 
        private float _shiftDownCoeff = 1;

        [SerializeField] 
        private float _cameraSwitchTime;
        
        private Squad.Squad _target;
        private float _distanceToTarget;
        private IDisposable _disposable;
        private Tweener _animation;

        public void Init(Squad.Squad owner)
        {
            _disposable?.Dispose();
            _target = owner;
            _distanceToTarget = _initialDistance;
            _disposable = _target.UnitsCount.Subscribe(it => OnTargetRadiusChanged());
        }

        private void Update()
        {
            var cameraTransform = UnityEngine.Camera.main.transform;
            var focusPos = transform.position - _shiftDownCoeff * _target.SquadRadius * Vector3.forward;
            cameraTransform.position = focusPos - _distanceToTarget * cameraTransform.forward;
        }
        
        private void OnDestroy()
        {
            _animation?.Kill();
            _animation = null;
            _disposable?.Dispose();
        }

        private void OnTargetRadiusChanged()
        {
            var newDistance = GetDistanceToTarget();
            if (Mathf.Abs(_distanceToTarget - newDistance) <= Mathf.Epsilon) return;
            
            PlayDistanceChangeAnimation(newDistance);
        }

        private void PlayDistanceChangeAnimation(float newDistance)
        {
            _animation?.Kill();
            _animation = DOTween.To(() => _distanceToTarget, value => { _distanceToTarget = value; }, newDistance,
                _cameraSwitchTime);
            _animation.OnComplete(() => _animation = null);
        }

        private float GetDistanceToTarget() => _initialDistance + _target.SquadRadius * _distanceIncreaseStep;
    }
}