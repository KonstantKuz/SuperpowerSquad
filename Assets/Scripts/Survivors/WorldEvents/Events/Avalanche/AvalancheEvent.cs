using System.Collections;
using Feofun.Extension;
using Survivors.Location;
using Survivors.Location.ObjectFactory;
using Survivors.Location.ObjectFactory.Factories;
using Survivors.Location.Service;
using UnityEngine;
using Zenject;

namespace Survivors.WorldEvents.Events.Avalanche
{
    public class AvalancheEvent : WorldEvent
    {
        private const int EMPTY_PLACE_SEARCH_ATTEMPT_COUNT = 5;
        private AvalancheEventConfig _config;
        
        [Inject] private World _world;
    
        [Inject]
        private ObjectInstancingFactory _objectFactory;  

        public override IEnumerator Start(EventConfig eventConfig)
        {
            _config = (AvalancheEventConfig) eventConfig;

            var lifeTime = 0f;
            while (true)
            {
                SpawnCobblestone();
                yield return new WaitForSeconds(_config.SpawnPeriod);
                lifeTime += _config.SpawnPeriod;
                if (lifeTime >= _config.EventDuration) yield break;
            }
        }
        private void SpawnCobblestone()
        {
            var stone = _objectFactory.Create<Cobblestone>(_config.CobblestonePrefab);
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
            var direction = Random.onUnitSphere.XZ();
            var position = _world.Squad.Position + direction * Random.Range(_config.MinDistanceFromPlayer, _config.MaxDistanceFromPlayer);
            var offset = Vector3.up * stone.Radius;
            position += offset;
            return position;
        }

        private bool IsPlaceBusy(Vector3 place)
        {
            return Physics.CheckSphere(place, _config.MinDistanceBtwnStones, _config.CobbleStoneMask);
        }
        protected override void Term()
        {
        }

    }
}
