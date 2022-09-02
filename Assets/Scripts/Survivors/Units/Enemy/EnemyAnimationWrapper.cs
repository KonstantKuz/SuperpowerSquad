using FSG.MeshAnimator;
using UnityEngine;

namespace Survivors.Units.Enemy
{
    public class EnemyAnimationWrapper: MonoBehaviour
    {
        [SerializeField]
        private MeshAnimatorBase _meshAnimator;
        
        [SerializeField]
        private bool crossFade = false;
        
        private void Start()
        {
            _meshAnimator.Play("walk");
            _meshAnimator.OnAnimationFinished += OnAnimationFinished;
        }
        private void OnAnimationFinished(string anim)
        {
            string newAnim = anim switch
            {
                "walk" => "walk",
                "attack" => "walk",
                _ => string.Empty
            };
            if (crossFade)
                _meshAnimator.Crossfade(newAnim);
            else
                _meshAnimator.Play(newAnim);
        }

        public void PlayAttack()
        {
            if (crossFade)
                _meshAnimator.Crossfade("attack");
            else
                _meshAnimator.Play("attack"); 
        }
    }
}