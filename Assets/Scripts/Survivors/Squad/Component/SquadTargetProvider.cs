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
        private struct TargetRecord
        {
            public ITarget Target;
            public float DistanceToSquad;
        }

        private const int SEARCH_COUNT_PER_UNIT = 20;
        private static UnitType TargetType => UnitType.ENEMY; 

        private Squad _squad;
        private List<TargetRecord> _targets = new List<TargetRecord>();

        [Inject] private TargetService _targetService;

        public IEnumerable<ITarget> Targets => _targets.Select(it => it.Target);

        public void Init(Squad owner)
        {
            _squad = owner;
        }

        public ITarget GetTargetBy(Vector3 position, float searchDistance)
        {
            var targets = _targets.Take(SEARCH_COUNT_PER_UNIT).Select(it => it.Target);
            return NearestTargetSearcher.Find(targets, position, searchDistance);
        }

        private void Update()
        {
            var squadPos = _squad.Destination.transform.position;
            _targets = _targetService.AllTargetsOfType(TargetType).Select(it => 
                new TargetRecord
                {
                    Target = it,
                    DistanceToSquad = Vector3.Distance(squadPos, it.Root.position)
                }).OrderBy(it => it.DistanceToSquad)
                .ToList();
        }
    }
}