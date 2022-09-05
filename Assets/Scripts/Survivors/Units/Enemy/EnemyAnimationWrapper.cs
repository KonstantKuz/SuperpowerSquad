using Survivors.Units.Component.Animation;

namespace Survivors.Units.Enemy
{
    public class EnemyAnimationWrapper : MeshAnimationWrapper
    {
        private const string IDLE_ANIMATION_NAME = "isIdle";
        private const string MOTION_ANIMATION_NAME = "isMotion";
        private const string ATTACK_ANIMATION_NAME = "attack";

        public void PlayIdle()
        {
            SetBool(MOTION_ANIMATION_NAME, false);
            SetBool(IDLE_ANIMATION_NAME, true);
        }

        public void PlayMoveForward()
        {
            SetBool(IDLE_ANIMATION_NAME, false);
            SetBool(MOTION_ANIMATION_NAME, true);
        }
        public void PlayAttack() => Play(ATTACK_ANIMATION_NAME);
    }
}