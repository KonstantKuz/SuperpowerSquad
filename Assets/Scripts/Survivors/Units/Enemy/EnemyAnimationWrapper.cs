using Feofun.Util.SerializableDictionary;
using FSG.MeshAnimator;
using UnityEngine;

namespace Survivors.Units.Enemy
{
    public class EnemyAnimationWrapper : MonoBehaviour
    {
        private const string ATTACK_ANIMATION_NAME = "attack";
        
        [SerializeField]
        private MeshAnimatorBase _meshAnimator;
        
        [SerializeField]
        private bool _crossFade = false;
        
        [SerializeField]
        private SerializableDictionary<string, string> _animationTransitions = new SerializableDictionary<string, string>();
        
        private void Start()
        {
            _meshAnimator.Play();
            _meshAnimator.OnAnimationFinished += OnAnimationFinished;
        }
        private void OnAnimationFinished(string animationName)
        {
            if (!_animationTransitions.ContainsKey(animationName)) {
                return;
            }
            var nextAnimation = _animationTransitions[animationName];
            Play(nextAnimation);
        }

        public void PlayAttack() => Play(ATTACK_ANIMATION_NAME);

        private void Play(string animationName)
        {
            if (_crossFade) {
                _meshAnimator.Crossfade(animationName);
            } else {
                _meshAnimator.Play(animationName);
            }
        }
    }
}