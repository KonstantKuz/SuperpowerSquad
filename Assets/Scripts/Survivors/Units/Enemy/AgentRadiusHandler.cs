using System;
using UnityEngine;
using UnityEngine.AI;

namespace Survivors.Units.Enemy
{
    public class AgentRadiusHandler
    {
        private readonly NavMeshAgent _agent;
        private readonly float _initialRadius;
        private readonly float _agentRadiusAfar;
        private readonly float _agentRadiusNear;
        private readonly float _agentDistanceNear;
        private readonly float _agentDistanceAfar;

        public AgentRadiusHandler(NavMeshAgent agent, float agentRadiusAfar, float agentRadiusNear, 
            float agentDistanceNear, float agentDistanceAfar)
        {
            _agent = agent;
            _initialRadius = _agent.radius;
            _agentRadiusAfar = agentRadiusAfar;
            _agentRadiusNear = agentRadiusNear;
            _agentDistanceNear = agentDistanceNear;
            _agentDistanceAfar = agentDistanceAfar;
        }

        public void UpdateRadius(float distanceToSquad, float scale)
        {
            _agent.radius = _initialRadius + Mathf.Lerp(_agentRadiusNear / scale, _agentRadiusAfar / scale, 
                                (distanceToSquad - _agentDistanceNear) / (_agentDistanceAfar - _agentDistanceNear));
        }
    }

    [Serializable]
    public class RadiusHanlderParams
    {
        public float AgentRadiusAfar;
        public float AgentRadiusNear;
        public float AgentDistanceNear;
        public float AgentDistanceAfar;
    }
}