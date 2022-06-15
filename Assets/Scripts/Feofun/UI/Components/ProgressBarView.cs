using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Feofun.UI.Components
{
    [RequireComponent(typeof(Image))]
    public class ProgressBarView : AnimatedValueView<float, Image> 
    {
        private Sequence _loopSequence;
        public bool IsIndependentUpdate { get; set; }
        public override float Value
        {
            get => Component.fillAmount;
            protected set
            {
                Component.fillAmount = Mathf.Clamp01(value);
            }
        }
        protected override Tweener Animate(float fromValue, float toValue, float time)
        {
            return DOTween.To(() => fromValue, value => { Value = value; }, toValue, time)
                          .SetUpdate(IsIndependentUpdate);
        }
        public void SetValueWithLoop(float toValue, bool forceLoop = false)
        {
            if (!_isValueInitialized) {
                Value = toValue;
                _isValueInitialized = true;
                return;
            }
            if (toValue > Value) {
                SetData(toValue);
                return;
            }
            if (!HaveLoop(toValue, forceLoop)) {
                return;
            }
            AnimateLooped(Value, toValue, _animationTime);
        }
        private bool HaveLoop(float toValue, bool forceLoop)
        {
            return (Math.Abs(Value - toValue) > Mathf.Epsilon || forceLoop);
        }
        private void AnimateLooped(float fromValue, float toValue, float time)
        {
            CancelLoopSequence();
            _loopSequence = DOTween.Sequence()
                                   .Append(Animate(fromValue, 1, time))
                                   .Append(Animate(0, toValue, time))
                                   .SetUpdate(IsIndependentUpdate);
        }

        private void CancelLoopSequence()
        {
            if (_loopSequence == null || !_loopSequence.IsActive() || !_loopSequence.IsPlaying()) return;
            _loopSequence.Kill();
            _loopSequence = null;
        }

        protected override void OnDestroy()
        {
            CancelLoopSequence();            
            base.OnDestroy();
        }
    }
}