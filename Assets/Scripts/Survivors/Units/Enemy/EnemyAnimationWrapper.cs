using Survivors.Units.Component.Animation;

namespace Survivors.Units.Enemy
{
    public class EnemyAnimationWrapper : MeshAnimationWrapper
    {
        private const string IDLE_ANIMATION_NAME = "idle";   
        private const string ATTACK_ANIMATION_NAME = "attack";      
        private const string MOTION_ANIMATION_NAME = "motion";

        public void PlayIdle() => Play(IDLE_ANIMATION_NAME);

        public void PlayMoveForward() => Play(ATTACK_ANIMATION_NAME);

        public void PlayAttack() => Play(MOTION_ANIMATION_NAME);
    }
}