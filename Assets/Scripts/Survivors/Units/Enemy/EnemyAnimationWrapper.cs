using Survivors.Units.Component.Animator;
using UnityEngine;

namespace Survivors.Units.Enemy
{
    public class EnemyAnimationWrapper : MonoBehaviour
    {
        private const string MOTION_ANIMATION_PARAM = "isMotion";
        private const string ATTACK_ANIMATION_NAME = "attack";

        [SerializeField]
        private AnimatorBase _animatorBase;
        
        public void PlayIdle() => _animatorBase.SetBool(MOTION_ANIMATION_PARAM, false);
        public void PlayMoveForward() => _animatorBase.SetBool(MOTION_ANIMATION_PARAM, true);
        public void PlayAttack() => _animatorBase.Play(ATTACK_ANIMATION_NAME);
    }
}