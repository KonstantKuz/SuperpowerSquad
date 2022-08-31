using DG.Tweening;
using UnityEngine;

namespace Survivors.Units.Component
{
    public class AnimationWrapper
    {
        private const float SMOOTH_TRANSITION_TIME = 0.2f;
        private readonly int _verticalMotionHash = Animator.StringToHash("VerticalMotion");
        private readonly Animator _animator;

        public AnimationWrapper(Animator animator)
        {
            _animator = animator;
        }

        public void PlayIdle()
        {
            _animator.SetFloat(_verticalMotionHash, 0, SMOOTH_TRANSITION_TIME, Time.deltaTime);
        }

        public void PlayMoveForward()
        {
            _animator.SetFloat(_verticalMotionHash, 1, SMOOTH_TRANSITION_TIME, Time.deltaTime);
        }
    }
}