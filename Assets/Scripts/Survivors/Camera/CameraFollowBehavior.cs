using System;
using DG.Tweening;
using Feofun.Components;
using UniRx;
using UnityEngine;

namespace Survivors.Camera
{
    public class CameraFollowBehavior : MonoBehaviour, IInitializable<Squad.Squad>
    {
        [SerializeField] 
        private float _initialDistance;
        [SerializeField] 
        private float _distanceMultiplier;

        [SerializeField] 
        private float _shiftDownCoeff = 1;

        [SerializeField] 
        private float _cameraSwitchTime;
        
        private Squad.Squad _squad;
        private float _distanceToObject;
        private IDisposable _disposable;
        private Tweener _animation;
        private float _initialSquadSize;

        public void Init(Squad.Squad owner)
        {
            _disposable?.Dispose();
            _squad = owner;
            _initialSquadSize = _squad.SquadRadius;
            _distanceToObject = _initialDistance;
            _disposable = _squad.UnitsCount.Subscribe(it => OnSquadSizeChanged());
        }

        private void Update()
        {
            var cameraTransform = UnityEngine.Camera.main.transform;
            var focusPos = transform.position - _shiftDownCoeff * _squad.SquadRadius * Vector3.forward;
            cameraTransform.position = focusPos - _distanceToObject * cameraTransform.forward;
        }

        private void OnDestroy()
        {
            _animation?.Kill();
            _animation = null;
            _disposable?.Dispose();
        }

        private void OnSquadSizeChanged()
        {
            _animation?.Kill();
            _animation = DOTween.To(() => _distanceToObject, value => { _distanceToObject = value; }, GetDistanceToObject(),
                _cameraSwitchTime);
            _animation.OnComplete(() => _animation = null);
        }

        private float GetDistanceToObject() => _initialDistance + (_squad.SquadRadius - _initialSquadSize) * _distanceMultiplier;
    }
}