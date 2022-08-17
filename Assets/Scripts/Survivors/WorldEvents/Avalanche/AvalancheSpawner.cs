using System.Collections;
using Survivors.Extension;
using Survivors.Location;
using Survivors.Location.Service;
using UnityEngine;
using Zenject;

namespace Survivors.WorldEvents.Avalanche
{
    public class AvalancheSpawner : MonoBehaviour
    {
        private const float MOVE_DIRECTION_DRIVEN_CHANCE = 0.5f;

        [SerializeField] private int _spawnCount;
        [SerializeField] private float _spawnPeriod;
        [SerializeField] private float _minDistanceBtwnStones;
        [SerializeField] private float _maxAngleSpread;
        [SerializeField] private float _minDistanceFromPlayer;
        [SerializeField] private float _maxDistanceFromPlayer;
        [SerializeField] private LayerMask _cobbleStoneMask;
        [SerializeField] private Cobblestone _cobblestonePrefab;

        private int _spawnedWaves;
        
        [Inject] private World _world;
        [Inject] private WorldObjectFactory _worldObjectFactory;

        private IEnumerator Start()
        {
            for (int i = 0; i < _spawnCount; i++)
            {
                SpawnCobblestone();
                yield return new WaitForSeconds(_spawnPeriod);
            }
        }

        private void SpawnCobblestone()
        {
            var stone = _worldObjectFactory.CreateObject(_cobblestonePrefab.gameObject).RequireComponent<Cobblestone>();
            stone.transform.position = GetRandomPlaceAroundPlayer() + Vector3.up * stone.Radius;
            var directionToPlayer = (_world.Squad.Position - stone.transform.position).XZ().normalized;
            directionToPlayer = Quaternion.Euler(0, Random.Range(-_maxAngleSpread, _maxAngleSpread), 0) * directionToPlayer;
            stone.Launch(directionToPlayer);
        }

        private Vector3 GetRandomPlaceAroundPlayer()
        {
            var direction = _world.Squad.IsMoving && Random.value < MOVE_DIRECTION_DRIVEN_CHANCE
                ? _world.Squad.MoveDirection
                : Random.onUnitSphere.XZ();
            var position = _world.Squad.Position + direction * Random.Range(_minDistanceFromPlayer, _maxDistanceFromPlayer);
            return !IsPlaceBusy(position) ? GetRandomPlaceAroundPlayer() : position;
        }

        private bool IsPlaceBusy(Vector3 place)
        {
            return Physics.CheckSphere(place, _minDistanceBtwnStones, _cobbleStoneMask);
        }
    }
}
