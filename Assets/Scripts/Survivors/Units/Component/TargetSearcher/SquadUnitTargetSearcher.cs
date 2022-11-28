using System.Collections.Generic;
using Feofun.Components;
using JetBrains.Annotations;
using Survivors.Squad.Component;
using Survivors.Units.Target;
using UnityEngine;

namespace Survivors.Units.Component.TargetSearcher
{
    public class SquadUnitTargetSearcher : MonoBehaviour, ITargetSearcher, IInitializable<IUnit>, IInitializable<Squad.Squad>
    {
        private IUnit _owner;
        private SquadTargetProvider _targetProvider;
        public void Init(IUnit owner)
        {
            _owner = owner;
        }

        public void Init(Squad.Squad owner)
        {
            _targetProvider = owner.TargetProvider;
        }

        [CanBeNull]
        public ITarget Find()
        {
            return _targetProvider.GetTargetBy(_owner.SelfTarget.Root.position);
        }
        
        public IEnumerable<ITarget> GetAllOrderedByDistance()
        {
            return _targetProvider.Targets;
        }
    }
}