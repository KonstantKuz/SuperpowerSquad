﻿using System.Collections;
using DG.Tweening;
using Survivors.Location.ObjectFactory;
using UnityEngine;
using Zenject;

namespace Survivors.Units.Component.Death
{
    public class DeathAnimation : MonoBehaviour, IUnitDeath
    { 
        private readonly int _deathHash = UnityEngine.Animator.StringToHash("Death");
        
        [SerializeField]
        private float _disappearTime;       
        [SerializeField]
        private float _delayUntilDisappear;     
        [SerializeField]
        private float _offsetYDisappear;
        
        private UnityEngine.Animator _animator;
        private Tweener _disappearTween;
        private Coroutine _disappear;
        
        [Inject(Id = ObjectFactoryType.Instancing)]
        private IObjectFactory _objectFactory;
        private void Awake()
        {
            _animator = GetComponentInChildren<UnityEngine.Animator>();
        }

        public void PlayDeath()
        {
            Dispose();
            _disappear = StartCoroutine(Disappear());
        }
        private IEnumerator Disappear()
        {
            EndAnimationIfStarted();
            
            _animator.SetTrigger(_deathHash);            
            yield return new WaitForSeconds(_delayUntilDisappear);
            _disappearTween = gameObject.transform.DOMoveY(transform.position.y - _offsetYDisappear, _disappearTime);
            yield return _disappearTween.WaitForCompletion(); 
            _objectFactory.Destroy(gameObject);
        }

        private void EndAnimationIfStarted()
        {
            if (_disappearTween == null) return;
            _disappearTween.Kill(true);
            _disappearTween = null;
        }

        private void Dispose()
        {
            if (_disappear != null) {
                StopCoroutine(_disappear);
                _disappear = null;
            }
            EndAnimationIfStarted();
        }

        private void OnDisable()
        {
            Dispose();
        }

     
    }
}
