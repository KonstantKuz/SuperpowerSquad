using DG.Tweening;
using UnityEngine;

namespace Feofun.UI.Components
{
    public abstract class AnimatedValueView<TValue, TComponent>: SingleComponentView<TComponent>
    {
        [SerializeField] 
        protected float _animationTime = 0.5f;
        protected Tweener _currentTween;
        protected bool _isValueInitialized;
        
        public void Reset() => _isValueInitialized = false;
        
        public void SetData(TValue value)
        {
            if (!_isValueInitialized)
            {
                Value = value;
                _isValueInitialized = true;
                return;
            }
            
            CancelAnimation();
            _currentTween = Animate(Value, value, _animationTime);
        }

        private void CancelAnimation()
        {
            if (_currentTween is { active: true } && _currentTween.IsPlaying())
            {
                _currentTween.Kill();
            }
        }

        public abstract TValue Value { get; protected set; }
        protected abstract Tweener Animate(TValue fromValue, TValue toValue, float time);

        protected virtual void OnDestroy()
        {
            CancelAnimation();
        }
    }
}