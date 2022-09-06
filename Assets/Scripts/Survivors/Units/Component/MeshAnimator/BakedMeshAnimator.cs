using System.Collections.Generic;
using System.Linq;
using Feofun.Util.SerializableDictionary;
using FSG.MeshAnimator;
using JetBrains.Annotations;
using ModestTree;
using UnityEngine;

namespace Survivors.Units.Component.MeshAnimator
{
    public class BakedMeshAnimator : MonoBehaviour
    {
        [SerializeField]
        private MeshAnimatorBase _meshAnimator;

        [SerializeField]
        private bool _crossFade = false;
        [SerializeField]
        private float _crossFadeSpeed = 0.2f;
        [SerializeField]
        private List<TransitionState> _animationTransitions = new List<TransitionState>();

        private readonly Dictionary<string, bool> _boolConditions = new SerializableDictionary<string, bool>();

        private Dictionary<string, List<TransitionState>> _transactions;
        private Dictionary<string, List<TransitionState>> Transactions =>
                _transactions ??= _animationTransitions.GroupBy(it => it.FromAnimation)
                                                       .ToDictionary(it => it.Key, it => it.ToList());

        private void Awake()
        {
            _meshAnimator.OnAnimationFinished += OnAnimationFinished;
        }

        public void Play(string animationName)
        {
            if (_crossFade) {
                _meshAnimator.Crossfade(animationName, _crossFadeSpeed);
            } else {
                _meshAnimator.Play(animationName);
            }
        }

        public void SetBool(string param, bool value)
        {
            _boolConditions[param] = value;
        }

        private void OnAnimationFinished(string animationName)
        {
            if (!Transactions.ContainsKey(animationName)) {
                return;
            }
            var nextAnimationState = FindTransaction(Transactions[animationName]);
            if (nextAnimationState != null) {
                Play(nextAnimationState.Value.ToAnimation);
            }
        }

        [CanBeNull]
        private TransitionState? FindTransaction(List<TransitionState> transitions)
        {
            foreach (var transitionState in transitions) {
                if (CanMoveToState(transitionState)) {
                    return transitionState;
                }
            }
            return null;
        }

        private bool CanMoveToState(TransitionState nextAnimationState)
        {
            if (nextAnimationState.BoolConditions == null || nextAnimationState.BoolConditions.IsEmpty()) {
                return true;
            }
            return nextAnimationState.BoolConditions.All(condition => {
                if (!_boolConditions.ContainsKey(condition.Key)) {
                    return false;
                }
                return _boolConditions[condition.Key] == condition.Value;
            });
        }

        private void OnDestroy()
        {
            _meshAnimator.OnAnimationFinished -= OnAnimationFinished;
        }
    }
}