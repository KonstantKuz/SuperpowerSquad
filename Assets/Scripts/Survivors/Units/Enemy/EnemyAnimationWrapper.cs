using Survivors.Units.Component.MeshAnimator;
using UnityEngine;

namespace Survivors.Units.Enemy
{
    public class EnemyAnimationWrapper : MonoBehaviour
    {
        private const string MOTION_ANIMATION_PARAM = "isMotion";
        private const string ATTACK_ANIMATION_NAME = "attack";

        [SerializeField]
        private BakedMeshAnimator _animator;

        public void PlayIdle() => _animator.SetBool(MOTION_ANIMATION_PARAM, false);
        public void PlayMoveForward() => _animator.SetBool(MOTION_ANIMATION_PARAM, true);
        public void PlayAttack() => _animator.Play(ATTACK_ANIMATION_NAME);
    }
}