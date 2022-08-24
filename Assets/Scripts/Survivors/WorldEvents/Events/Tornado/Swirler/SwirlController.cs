using System;
using System.Collections;
using Survivors.Extension;
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
        
        private float _passedTime;

        private bool _timeoutCompleted = true;
        private bool IsAttached => _tornado != null;

        private void Awake()
        {
            _owner = gameObject.RequireComponentInParent<IMovementLockable>();
        }
        public void AttachToTornado(GameObject tornado)
        {
            if (IsAttached || !_timeoutCompleted) {
                return;
            }
            _tornado = tornado;
            _disposable = _tornado.OnDestroyAsObservable().Subscribe((o) => ReleaseFromTornado());
            _passedTime = 0;
            _owner.Lock();
            _timeoutCompleted = false;
        } 
        private void ReleaseFromTornado()
        {
            if (!IsAttached) {
                return;
            }
            _owner.UnLock();
            _tornado = null;
            Dispose();
            _passedTime = 0;
            StartCoroutine(StartTimeoutAfterRelease());
        }

        private void Dispose()
        {
            _disposable?.Dispose();
            _disposable = null;
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
    }
    
}