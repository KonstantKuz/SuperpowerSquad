using Feofun.Util.SerializableDictionary;
using FSG.MeshAnimator;
using UnityEngine;

namespace Survivors.Units.Component.Animation
{
    public class MeshAnimationWrapper : MonoBehaviour
    {
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
        
        public void Play(string animationName)
        {
            if (_crossFade) {
                _meshAnimator.Crossfade(animationName);
            } else {
                _meshAnimator.Play(animationName);
            }
        }
        private void OnAnimationFinished(string animationName)
        {
            if (!_animationTransitions.ContainsKey(animationName)) {
                return;
            }
            var nextAnimation = _animationTransitions[animationName];
            Play(nextAnimation);
        }

        private void OnDestroy()
        {
            _meshAnimator.OnAnimationFinished -= OnAnimationFinished;
        }
    }
}