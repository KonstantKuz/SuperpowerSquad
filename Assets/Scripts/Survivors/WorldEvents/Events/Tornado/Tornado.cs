using DG.Tweening;
using Survivors.Location.Model;
using Survivors.WorldEvents.Events.Tornado.Config;
using UnityEngine;

namespace Survivors.WorldEvents.Events.Tornado
{
    public class Tornado : WorldObject
    {
        private TornadoEventConfig _config;
        
        private Tween _appearTween;
        private Tween _disappearTween;

        private Vector3 _initialScale;

        private void Awake()
        {
            _initialScale = transform.localScale;
            transform.localScale = Vector3.zero;
        }

        public void Init(TornadoEventConfig config)
        {
            DisposeTween();
            _config = config;
            _appearTween = transform.DOScale(_initialScale, _config.RandomAppearTime);
        }
        public void Dispose()
        {
            _disappearTween = transform.DOScale(Vector3.zero, _config.RandomDisappearTime);
            _disappearTween.onComplete = () => {
                Destroy(gameObject);
                _disappearTween = null;
            };
        }
        private void DisposeTween()
        {
            _appearTween?.Kill(true); 
            _disappearTween?.Kill(true);
        }
        private void OnDisable()
        {
            DisposeTween();
        }
    }
}