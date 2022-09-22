using System.Collections.Generic;
using System.Linq;
using Feofun.Util.SerializableDictionary;
using FSG.MeshAnimator;
using JetBrains.Annotations;
using ModestTree;
using UnityEngine;

namespace Survivors.Units.Component.Animator.MeshAnimator
{
    public class BakedMeshAnimator : AnimatorBase
    {
        [SerializeField]
        private MeshAnimatorBase _meshAnimator;

        [SerializeField]
        private bool _crossFade = false;
        [SerializeField]
        private float _crossFadeSpeed = 0.2f;
        [SerializeField]
        private List<AnimationTransition> _animationTransitions = new List<AnimationTransition>();

        private readonly Dictionary<string, bool> _boolConditions = new SerializableDictionary<string, bool>();

        private Dictionary<string, List<AnimationTransition>> _transitionsMap;
        private Dictionary<string, List<AnimationTransition>> TransitionsMap =>
                _transitionsMap ??= _animationTransitions.GroupBy(it => it.FromAnimation).ToDictionary(it => it.Key, it => it.ToList());

        private void Awake()
        {
            _meshAnimator.OnAnimationFinished += OnAnimationFinished;
        }

        public override void Play(string animationName)
        {
            if (_crossFade) {
                _meshAnimator.Crossfade(animationName, _crossFadeSpeed);
            } else {
                _meshAnimator.Play(animationName);
            }
        }

        public override void SetBool(string param, bool value)
        {
            _boolConditions[param] = value;
        }

        private void OnAnimationFinished(string animationName)
        {
            var transition = FindTransition(animationName);
            if (transition != null) {
                Play(transition.Value.ToAnimation);
            }
        }

        [CanBeNull]
        private AnimationTransition? FindTransition(string animationName)
        {
            if (!TransitionsMap.ContainsKey(animationName)) {
                return null;
            }
            foreach (var transition in TransitionsMap[animationName]) {
                if (IsTransitionActive(transition)) {
                    return transition;
                }
            }
            return null;
        }

        private bool IsTransitionActive(AnimationTransition transition)
        {
            if (transition.BoolConditions == null || transition.BoolConditions.IsEmpty()) {
                return true;
            }
            return transition.BoolConditions.All(condition => IsRightCondition(condition.Key, condition.Value));
        }

        private bool IsRightCondition(string conditionName, bool conditionValue)
        {
            if (!_boolConditions.ContainsKey(conditionName)) {
                return false;
            }
            return _boolConditions[conditionName] == conditionValue;
        }

        private void OnDestroy()
        {
            _meshAnimator.OnAnimationFinished -= OnAnimationFinished;
        }
    }
}