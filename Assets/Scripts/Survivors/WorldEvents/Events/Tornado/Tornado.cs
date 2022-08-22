using System.Collections;
using DG.Tweening;
using Survivors.Location.Model;
using Survivors.WorldEvents.Events.Tornado.Config;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Survivors.WorldEvents.Events.Tornado
{
    public class Tornado : WorldObject
    {
        private TornadoEventConfig _config;
        
        private Tween _appearTween;
        private Tween _disappearTween;
        
        private Coroutine _directionCoroutine;
        
        public void Init(TornadoEventConfig config)
        {
            Dispose();
            _config = config;
            transform.localScale = Vector3.zero;

            _appearTween = transform.DOScale(Vector3.one, _config.RandomAppearTime);
            _directionCoroutine = StartCoroutine(StartDirectionChangeTimer());
            _appearTween.onComplete = () => {
                _appearTween = null;
            };
        }

        private void OnTriggerEnter(Collider other)
        {
            var swirler = other.GetComponentInParent<Swirler.Swirler>();
            if (swirler != null) {
                swirler.RunSwirl(gameObject);
            }
        }
        private IEnumerator StartDirectionChangeTimer()
        {
            while (true) {
                yield return new WaitForSeconds(_config.DirectionChangeTimeout);
                ChangeDirection();
            }
        }
        private void ChangeDirection()
        {
            transform.localRotation *= Quaternion.Euler(0, Random.Range(-_config.MaxRandomAngle, _config.MaxRandomAngle), 0);
        }
        private void Update()
        {
            transform.position += transform.forward * _config.Speed * Time.deltaTime;
        }

        public void Term()
        {
            _disappearTween = transform.DOScale(Vector3.zero, _config.RandomDisappearTime);
            _disappearTween.onComplete = () => {
                Destroy(gameObject);
                _disappearTween = null;
            };
        }
        private void DisposeCoroutine()
        {
            if (_directionCoroutine == null) {
                return;
            }
            StopCoroutine(_directionCoroutine);
            _directionCoroutine = null;
        }
        private void DisposeTween()
        {
            _appearTween?.Kill(true); 
            _disappearTween?.Kill(true);
        }

        private void Dispose()
        {
            DisposeTween();
            DisposeCoroutine();
        }

        private void OnDisable()
        {
            Dispose();
        }
    }
}