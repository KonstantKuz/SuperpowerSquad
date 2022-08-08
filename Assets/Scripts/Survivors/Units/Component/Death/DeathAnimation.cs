using System.Collections;
using DG.Tweening;
using Feofun.Components;
using Survivors.Location.Service;
using UnityEngine;
using Zenject;

namespace Survivors.Units.Component.Death
{
    public class DeathAnimation : MonoBehaviour, IUnitDeath, IInitializable<IUnit>
    { 
        private readonly int _deathHash = Animator.StringToHash("Death");
        
        [SerializeField]
        private float _disappearTime;       
        [SerializeField]
        private float _delayUntilDisappear;     
        [SerializeField]
        private float _offsetYDisappear;

        [Inject]
        private WorldObjectFactory _worldObjectFactory;

        private IUnit _owner;
        private Animator _animator;
        private Tweener _disappearTween;
        private Coroutine _disappear;
        public void Init(IUnit owner)
        {
            _owner = owner;
        }
        private void Awake()
        {
            _animator = GetComponentInChildren<Animator>();
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
            _worldObjectFactory.DestroyObject((Unit) _owner);
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
