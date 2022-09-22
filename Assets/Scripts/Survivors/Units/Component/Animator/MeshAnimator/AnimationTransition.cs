using System;
using Feofun.Util.SerializableDictionary;
using UnityEngine;

namespace Survivors.Units.Component.Animator.MeshAnimator
{
    [Serializable]
    public struct AnimationTransition
    {
        [SerializeField]
        private string _fromAnimation;
        [SerializeField]
        private string _toAnimation;
        [SerializeField]
        private SerializableDictionary<string, bool> _boolConditions;
        
        public string FromAnimation => _fromAnimation;
        public string ToAnimation => _toAnimation;
        public SerializableDictionary<string, bool> BoolConditions => _boolConditions;
    }
}