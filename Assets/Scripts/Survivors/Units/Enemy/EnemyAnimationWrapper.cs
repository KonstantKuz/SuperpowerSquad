using FSG.MeshAnimator;
using UnityEngine;

namespace Survivors.Units.Enemy
{
    public class EnemyAnimationWrapper: MonoBehaviour
    {
        private const string WALK_ANIMATION_NAME = "walk";
        private const string ATTACK_ANIMATION_NAME = "attack";
        
        [SerializeField]
        private MeshAnimatorBase _meshAnimator;
        
        [SerializeField]
        private bool _crossFade = false;
        
        private void Start()
        {
            _meshAnimator.Play();
            _meshAnimator.OnAnimationFinished += OnAnimationFinished;
        }
        private void OnAnimationFinished(string animationName)
        {
            var newAnim = string.Empty;
            switch (animationName)
            {
                case ATTACK_ANIMATION_NAME:
                    newAnim = WALK_ANIMATION_NAME;
                    break;
                default:
                    return;
            }
            Play(newAnim);
        }

        public void PlayAttack() => Play(ATTACK_ANIMATION_NAME);

        private void Play(string animationName)
        {
            if (_crossFade)
                _meshAnimator.Crossfade(animationName);
            else
                _meshAnimator.Play(animationName); 
        }
    }
}