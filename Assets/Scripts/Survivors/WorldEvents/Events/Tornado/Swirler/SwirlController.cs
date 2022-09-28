using System;
using System.Collections;
using Feofun.Extension;
using Survivors.Units.Component;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace Survivors.WorldEvents.Events.Tornado.Swirler
{
    public class SwirlController : MonoBehaviour
    {
        [SerializeField] private float _moveSpeed = 9;

        [SerializeField] private float _swirlDuration = 1f;

        [SerializeField] private float _moveAngle = 90;
        
        [SerializeField] private float _distanceForSwirl = 1.5f;
     
        [SerializeField] private float _timeoutAfterRelease = 1.5f; 
        
        private IMovementLockable _owner;
        
        private GameObject _tornado;
        private IDisposable _disposable;
        private Coroutine _timeoutCoroutine;
        
        private float _passedTime;

        private bool _timeoutCompleted = true;
        private bool IsAttached => _tornado != null;
        private bool CanAttach => !IsAttached && _timeoutCompleted;

        private void Awake()
        {
            _owner = gameObject.RequireComponentInParent<IMovementLockable>();
        }
        
        public void AttachToTornado(GameObject tornado)
        {
            if (!CanAttach) {
                return;
            }
            
            Dispose();
            
            _owner.Lock();
            _timeoutCompleted = false;
            
            _tornado = tornado;
            _disposable = _tornado.OnDestroyAsObservable().Subscribe(it => ReleaseFromTornado());
        }
        
        private void ReleaseFromTornado()
        {
            UnlockOwner();
            Dispose();
            _timeoutCoroutine = StartCoroutine(StartTimeoutAfterRelease());
        }
        
        private void UnlockOwner()
        {
            if (!IsAttached) return;
            _owner.UnLock();
        }
        
        private IEnumerator StartTimeoutAfterRelease()
        {
            yield return new WaitForSeconds(_timeoutAfterRelease);
            _timeoutCompleted = true;
        }
        private void Update()
        {
            if (!IsAttached) {
                return;
            }
            _passedTime += Time.deltaTime;
            UpdateMove();
            if (CanRelease()) {
                ReleaseFromTornado();
            }
        }

        private void UpdateMove()
        {
            var moveDirection = (_tornado.transform.position - transform.position).normalized;
            if (CanSwirl()) {
                moveDirection = Quaternion.Euler(0, _moveAngle, 0) * moveDirection;
            }
            transform.position += moveDirection * _moveSpeed * Time.deltaTime;
        }

        private bool CanRelease() => _passedTime > _swirlDuration;
        
        private bool CanSwirl() => Vector3.Distance(transform.position, _tornado.transform.position) < _distanceForSwirl;

        private void OnDisable()
        {
            UnlockOwner();
            Dispose();
        }

        private void OnDestroy()
        {
            Dispose();
        }
        
        private void Dispose()
        {
            _tornado = null;
            _passedTime = 0;

            _disposable?.Dispose();
            _disposable = null;
            
            if (_timeoutCoroutine == null) {
                return;
            }
            
            _timeoutCompleted = true;
            StopCoroutine(_timeoutCoroutine);
            _timeoutCoroutine = null;
        }
    }
}