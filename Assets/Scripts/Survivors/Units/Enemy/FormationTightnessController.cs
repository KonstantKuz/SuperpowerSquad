using System;
using Feofun.Components;
using Survivors.Extension;
using UnityEngine;
using UnityEngine.AI;

namespace Survivors.Units.Enemy
{
    public class FormationTightnessController : MonoBehaviour, IUpdatableComponent
    {
        [SerializeField] private float _agentRadiusAfar;
        [SerializeField] private float _agentRadiusNear;
        [SerializeField] private float _agentDistanceAfar;
        [SerializeField] private float _agentDistanceNear;

        private EnemyAi _enemyAi;
        private NavMeshAgent _agent;
        private float _initialRadius;

        private void Awake()
        {
            _enemyAi = gameObject.RequireComponent<EnemyAi>();
            _agent = gameObject.RequireComponent<NavMeshAgent>();
            _initialRadius = _agent.radius;
        }

        public void OnTick()
        {
            UpdateRadius();
        }

        private void UpdateRadius()
        {
            _agent.radius = _initialRadius + Mathf.Lerp(_agentRadiusNear / _enemyAi.Scale, _agentRadiusAfar / _enemyAi.Scale, 
                                (_enemyAi.DistanceToSquad - _agentDistanceNear) / (_agentDistanceAfar - _agentDistanceNear));
        }
    }
}