using System;
using System.Collections.Generic;
using System.Linq;
using Feofun.Util.SerializableDictionary;
using FSG.MeshAnimator;
using JetBrains.Annotations;
using ModestTree;
using UnityEngine;

namespace Survivors.Units.Component.Animation
{
    [Serializable]
    public class TransitionState
    {
        [SerializeField]
        public string ToAnimation;
        [SerializeField]
        public SerializableDictionary<string, bool> BoolConditions  = new SerializableDictionary<string, bool>();
    }
    

    public class MeshAnimationWrapper : MonoBehaviour
    {
        [SerializeField]
        private MeshAnimatorBase _meshAnimator;

        [SerializeField]
        private bool _crossFade = false;

        [SerializeField]
        private SerializableDictionary<string, List<TransitionState>> _animationTransitions = new SerializableDictionary<string, List<TransitionState>>();

        private Dictionary<string, bool> _boolConditions = new SerializableDictionary<string, bool>();

        private void Awake()
        {
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

        public void SetBool(string param, bool value)
        {
            _boolConditions[param] = value;
        }

        private void OnAnimationFinished(string animationName)
        {
            if (!_animationTransitions.ContainsKey(animationName)) {
                return;
            }
            var transitions = _animationTransitions[animationName];
            var transcation = GetTransaction(transitions);
            if (transcation != null) {
                Play(transcation.ToAnimation); 
            }
    

        }
        [CanBeNull]
        private TransitionState GetTransaction(List<TransitionState> transition)
        {
            return transition.FirstOrDefault(CanTransaction);
        }

        private bool CanTransaction(TransitionState nextAnimationState)
        {
            if (nextAnimationState.BoolConditions == null || nextAnimationState.BoolConditions.IsEmpty()) {
                return true;
            }
            return nextAnimationState.BoolConditions.All(it => {
                if (!_boolConditions.ContainsKey(it.Key)) {
                    return false;
                }
                return _boolConditions[it.Key] == it.Value;
            });
        }

        private void OnDestroy()
        {
            _meshAnimator.OnAnimationFinished -= OnAnimationFinished;
        }
    }
}