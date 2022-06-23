using System.Collections.Generic;
using System.Linq;
using Feofun.Components;
using Survivors.Units.Model;
using Survivors.Units.Player.Model;
using Survivors.Units.Target;
using UnityEngine;
using Zenject;

namespace Survivors.Units.Component.TargetSearcher
{
    public class ClusterTargetSearcher : MonoBehaviour, ITargetSearcher, IInitializable<IUnit>
    {
        private const int MAX_COLLIDERS = 100;
        
        [Inject]
        private TargetService _targetService;
        
        private UnitType _targetType;
        private float _clusterRadius;
        private float _searchDistance;
        private readonly int _enemyLayer = LayerMask.GetMask("Enemy");
        
        private readonly Collider[] _hitColliders = new Collider[MAX_COLLIDERS];

        public ITarget Find()
        {
            var targets = _targetService.AllTargetsOfType(_targetType).ToList();
            var pos = transform.position;
            var bestTarget = NearestTargetSearcher.Find(targets, pos, _searchDistance);
            var bestTargetScore = -1;
            
            foreach (var target in targets)
            {
                if (Vector3.Distance(target.Root.position, pos) > _searchDistance) continue;
                if (!target.IsAlive) continue;
                var score = Physics.OverlapSphereNonAlloc(target.Root.position, _clusterRadius, _hitColliders, _enemyLayer);
                if (score <= bestTargetScore) continue;
                
                bestTarget = target;
                bestTargetScore = score;
            }

            return bestTarget;
        }

        public IEnumerable<ITarget> GetAllOrderedByDistance()
        {
            throw new System.NotImplementedException();
        }

        public void Init(IUnit owner)
        {
            _targetType = owner.UnitType.GetTargetUnitType();
            _searchDistance = owner.Model.AttackModel.AttackDistance;
            _clusterRadius = (owner.Model.AttackModel as PlayerAttackModel).DamageRadius;
        }
    }
}