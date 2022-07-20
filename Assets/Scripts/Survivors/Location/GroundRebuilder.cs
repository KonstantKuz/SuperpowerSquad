using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

namespace Survivors.Location
{
    public class GroundRebuilder : MonoBehaviour
    {
        [SerializeField] private float _closestEdgeDistanceForRebuild = 20;
        [SerializeField] private float _updatePeriod = 1;

        [Inject] private World _world;
        
        private float _timer;
        private Transform Ground => _world.Ground;
        [CanBeNull] private Squad.Squad Squad => _world.Squad;
        private void Update()
        {
            if (_world.Squad == null) {
                return;
            }
            _timer += Time.deltaTime;
            if (_timer < _updatePeriod) return;
            _timer = 0f;
            CheckBoundsGround();
        }

        private void CheckBoundsGround()
        {
            if (_world.Squad == null) {
                return;
            }
            if (!NavMesh.FindClosestEdge(Squad.Destination.transform.position, out var hit, NavMesh.AllAreas)) {
                return;
            }
            if (hit.distance < _closestEdgeDistanceForRebuild) {
                RebuildGround();
            }
        }
        private void RebuildGround()
        {
            if (_world.Squad == null) {
                return;
            }
            _world.Pause();
            Ground.position = Squad.Destination.transform.position;
            _world.RebuildNavMesh();
            _world.UnPause();
        }
    }
}