using DG.Tweening;
using UnityEngine;

namespace Survivors.Units.Component
{
    public class MoveAnimationWrapper
    {
        private const float SMOOTH_TRANSITION_TIME = 0.2f;
        private readonly int _verticalMotionHash = Animator.StringToHash("VerticalMotion");
        private readonly Animator _animator;

        private Tween _currentTransition;

        public MoveAnimationWrapper(Animator animator)
        {
            _animator = animator;
        }

        public void PlayIdleSmooth()
        {
            if (_animator.GetFloat(_verticalMotionHash) == 0) return;
            
            _currentTransition?.Kill();
            _currentTransition = SmoothTransition(_verticalMotionHash, 0, SMOOTH_TRANSITION_TIME);
        }

        public void PlayMoveForwardSmooth()
        {
            if (_animator.GetFloat(_verticalMotionHash) == 1) return;
            
            _currentTransition?.Kill();
            _currentTransition = SmoothTransition(_verticalMotionHash, 1, SMOOTH_TRANSITION_TIME);
        }

        private Tween SmoothTransition(int animationHash, float toValue, float time)
        {
            return DOTween.To(() => _animator.GetFloat(animationHash), value => { _animator.SetFloat(animationHash, value); }, toValue, time);
        }
    }
}