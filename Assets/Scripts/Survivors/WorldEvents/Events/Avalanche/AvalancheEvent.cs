using System.Collections;
using Survivors.Extension;
using Survivors.Location;
using Survivors.Location.Service;
using UnityEngine;
using Zenject;

namespace Survivors.WorldEvents.Events.Avalanche
{
    public class AvalancheEvent : WorldEvent
    {
        private const int EMPTY_PLACE_SEARCH_ATTEMPT_COUNT = 5;
        private AvalancheEventConfig _config;
        private bool _isLive;
        
        [Inject] private World _world;
        [Inject] private WorldObjectFactory _worldObjectFactory;

        public override IEnumerator Start(EventConfig eventConfig)
        {
            _config = (AvalancheEventConfig) eventConfig;
            _isLive = true;
            while (_isLive)
            {
                SpawnCobblestone();
                yield return new WaitForSeconds(_config.SpawnPeriod);
            }
        }

        private void SpawnCobblestone()
        {
            var stone = _worldObjectFactory.CreateObject(_config.CobblestonePrefab).RequireComponent<Cobblestone>();
            stone.transform.position = GetEmptyRandomPlace(stone);
            
            var directionToPlayer = GetDirectionToPlayerFor(stone);
            stone.Launch(directionToPlayer, _config.StoneDamagePercent);
        }

        private Vector3 GetDirectionToPlayerFor(Cobblestone stone)
        {
            var directionToPlayer = (_world.Squad.Position - stone.transform.position).XZ().normalized;
            var angle = Quaternion.Euler(0, Random.Range(-_config.MaxAngleSpread, _config.MaxAngleSpread), 0);
            return angle * directionToPlayer;
        }

        private Vector3 GetEmptyRandomPlace(Cobblestone stone)
        {
            for (int i = 0; i < EMPTY_PLACE_SEARCH_ATTEMPT_COUNT; i++)
            {
                var place = GetRandomPlaceFor(stone);
                if (!IsPlaceBusy(place))
                {
                    return place;
                }
            }

            return GetRandomPlaceFor(stone);
        }
        
        private Vector3 GetRandomPlaceFor(Cobblestone stone)
        {
            var direction = _world.Squad.IsMoving && Random.value < _config.MoveDirectionDrivenChance
                ? _world.Squad.MoveDirection
                : Random.onUnitSphere.XZ();
            var position = _world.Squad.Position + direction * Random.Range(_config.MinDistanceFromPlayer, _config.MaxDistanceFromPlayer);
            var offset = Vector3.up * stone.Radius;
            position += offset;
            return position;
        }

        private bool IsPlaceBusy(Vector3 place)
        {
            return Physics.CheckSphere(place, _config.MinDistanceBtwnStones, _config.CobbleStoneMask);
        }
        
        protected override void Dispose()
        {
            _isLive = false;
        }
    }
}
