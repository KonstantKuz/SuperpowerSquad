using System.Collections;
using Survivors.Extension;
using Survivors.Location;
using Survivors.Location.Service;
using Survivors.WorldEvents.Events;
using UnityEngine;
using Zenject;

namespace Survivors.WorldEvents.Avalanche
{
    public class AvalancheEvent : WorldEvent
    {
        private AvalancheEventConfig _config;
        private float _startTime;
        
        [Inject] private World _world;
        [Inject] private WorldObjectFactory _worldObjectFactory;

        private bool IsLive => Time.time < _startTime + _config.EventDuration;
        
        public override IEnumerator Start(EventConfig eventConfig)
        {
            _config = (AvalancheEventConfig) eventConfig;
            _startTime = Time.time;
            while (IsLive)
            {
                SpawnCobblestone();
                yield return new WaitForSeconds(_config.SpawnPeriod);
            }
        }

        private void SpawnCobblestone()
        {
            var stone = _worldObjectFactory.CreateObject(_config.CobblestonePrefab).RequireComponent<Cobblestone>();
            stone.transform.position = GetRandomPlaceAroundPlayer() + Vector3.up * stone.Radius;
            var directionToPlayer = (_world.Squad.Position - stone.transform.position).XZ().normalized;
            directionToPlayer = Quaternion.Euler(0, Random.Range(-_config.MaxAngleSpread, _config.MaxAngleSpread), 0) * directionToPlayer;
            stone.Launch(directionToPlayer);
        }

        private Vector3 GetRandomPlaceAroundPlayer()
        {
            var direction = _world.Squad.IsMoving && Random.value < _config.MoveDirectionDrivenChance
                ? _world.Squad.MoveDirection
                : Random.onUnitSphere.XZ();
            var position = _world.Squad.Position + direction * Random.Range(_config.MinDistanceFromPlayer, _config.MaxDistanceFromPlayer);
            return !IsPlaceBusy(position) ? GetRandomPlaceAroundPlayer() : position;
        }

        private bool IsPlaceBusy(Vector3 place)
        {
            return Physics.CheckSphere(place, _config.MinDistanceBtwnStones, _config.CobbleStoneMask);
        }
        
        protected override void Dispose()
        {
        }
    }
}
