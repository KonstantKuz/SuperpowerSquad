using System.Collections;
using UnityEngine;

namespace Survivors.WorldEvents.Events.Tornado.Swirler
{
    public abstract class Swirler : MonoBehaviour
    {
        [SerializeField] private float _moveSpeed = 9;

        [SerializeField] private float _swirlDuration = 1f;

        [SerializeField] private float _moveAngle = 90;
        
        [SerializeField] private float _distanceForSwirl = 1.5f;
     
        [SerializeField] private float _timeoutAfterStop = 1.5f; 
        

        private GameObject _swirlCenter;
        
        private float _passedTime;

        private bool Running { get; set; }

        protected abstract void Capture();  
        
        protected abstract void Release();

        public void RunSwirl(GameObject swirlCenter)
        {
            if (_swirlCenter != null || Running) {
                return;
            }
            _swirlCenter = swirlCenter;
            _passedTime = 0;
            Capture();
            Running = true;
        }
        private void StopSwirl()
        {
            Release();
            _swirlCenter = null;
            _passedTime = 0;
            StartCoroutine(StartTimeoutAfterStop());
        }
        private IEnumerator StartTimeoutAfterStop()
        {
            yield return new WaitForSeconds(_timeoutAfterStop);
            Running = false;
        }

        private void Update()
        {
            if (_swirlCenter == null) {
                Release();
                return;
            }
            _passedTime += Time.deltaTime;
            UpdateMove();
            if (CanStop()) {
                StopSwirl();
            }
        }

        private void UpdateMove()
        {
            var moveDirection = (_swirlCenter.transform.position - transform.position).normalized;
            if (CanSwirl()) {
                moveDirection = Quaternion.Euler(0, _moveAngle, 0) * moveDirection;
            }
            transform.position += moveDirection * _moveSpeed * Time.deltaTime;
        }

        private bool CanStop() => _passedTime > _swirlDuration;
        
        private bool CanSwirl() => Vector3.Distance(transform.position, _swirlCenter.transform.position) < _distanceForSwirl;
    }
    
}