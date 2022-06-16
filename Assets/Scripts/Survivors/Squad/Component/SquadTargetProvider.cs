using System;
using System.Collections.Generic;
using System.Linq;
using Feofun.Components;
using Survivors.Units;
using Survivors.Units.Component.TargetSearcher;
using Survivors.Units.Target;
using UnityEngine;
using Zenject;

namespace Survivors.Squad.Component
{
    public class SquadTargetProvider : MonoBehaviour, IInitializable<Squad>
    {
        private const int SEARCH_COUNT_PER_UNIT = 20;
        private static UnitType TargetType => UnitType.ENEMY; 

        private Squad _squad;
        private List<ITarget> _targets = new List<ITarget>();

        [Inject] private TargetService _targetService;

        public List<ITarget> Targets => _targets;

        public void Init(Squad owner)
        {
            _squad = owner;
        }

        public ITarget GetTargetBy(Vector3 position, float searchDistance)
        {
            var targets = _targets.Take(SEARCH_COUNT_PER_UNIT);
            return NearestTargetSearcher.Find(targets, position, searchDistance);
        }

        private void Update()
        {
            _targets = _targetService.AllTargetsOfType(TargetType).OrderBy(DistanceToSquad).ToList();
        }

        private float DistanceToSquad(ITarget target)
        {
            return Vector3.Distance(_squad.Destination.transform.position, target.Root.position);
        }
    }
}