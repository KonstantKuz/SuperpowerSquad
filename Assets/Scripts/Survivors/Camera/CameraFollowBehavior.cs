﻿using System;
using DG.Tweening;
using Feofun.Components;
using UniRx;
using UnityEngine;

namespace Survivors.Camera
{
    public class CameraFollowBehavior : MonoBehaviour, IInitializable<Squad.Squad>
    {
        [SerializeField] 
        private float _sizeToDistanceCoeff;

        [SerializeField] 
        private float _cameraSwitchTime;
        
        private Squad.Squad _squad;
        private float _distanceToObject;
        private IDisposable _disposable;
        private Tweener _animation;

        public void Init(Squad.Squad owner)
        {
            _disposable?.Dispose();
            _squad = owner;
            _distanceToObject = GetDistanceToObject();
            _disposable = _squad.UnitsCount.Subscribe(it => OnSquadSizeChanged());
        }

        private void Update()
        {
            var cameraTransform = UnityEngine.Camera.main.transform;
            cameraTransform.position = transform.position - _distanceToObject * cameraTransform.forward;
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

        private float GetDistanceToObject() => _squad.SquadRadius * _sizeToDistanceCoeff;
    }
}