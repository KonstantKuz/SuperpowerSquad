using Feofun.Components;
using Feofun.Extension;
using Survivors.Location;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

namespace Survivors.Units.Enemy
{
    public class OutOfViewAcceleration : MonoBehaviour, IUpdatableComponent
    {
        [SerializeField] private float _speedMultiplier = 5f;
        [SerializeField] private float _accelerationDistanceToSquad = 20f;
        
        private NavMeshAgent _agent;
        private float? _initialSpeed;

        [Inject] private World _world;
        
        private NavMeshAgent Agent => _agent ??= gameObject.RequireComponent<NavMeshAgent>();
        private float InitialSpeed => _initialSpeed ??= _agent.speed;
        private float DistanceToSquad => Vector3.Distance(_world.Squad.Position, transform.position);
        
        public void OnTick()
        {
            Agent.speed = DistanceToSquad >= _accelerationDistanceToSquad
                ? InitialSpeed * _speedMultiplier
                : InitialSpeed;
        }
    }
}