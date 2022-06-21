using Survivors.Enemy.Spawn.Config;
using Survivors.Location;
using UnityEngine;

namespace Survivors.Enemy.Spawn.PlaceProviders
{
    public class CompositeSpawnPlaceProvider : ISpawnPlaceProvider
    {
        private const float MOVE_DIRECTION_DRIVEN_CHANCE = 0.5f;
        
        private readonly ISpawnPlaceProvider _randomDrivenProvider;
        private readonly ISpawnPlaceProvider _moveDirectionDrivenProvider;

        public CompositeSpawnPlaceProvider(EnemyWavesSpawner wavesSpawner, World world)
        {
            _randomDrivenProvider = new RandomDrivenPlaceProvider(wavesSpawner, world);
            _moveDirectionDrivenProvider = new MoveDirectionDrivenPlaceProvider(wavesSpawner, world.Squad);
        }
        
        public SpawnPlace GetSpawnPlace(EnemyWaveConfig waveConfig, int rangeTry)
        {
            if (Random.value < MOVE_DIRECTION_DRIVEN_CHANCE)
            {
                var spawnPlace = _moveDirectionDrivenProvider.GetSpawnPlace(waveConfig, rangeTry);
                if (spawnPlace.IsValid) return spawnPlace;
            }
            return _randomDrivenProvider.GetSpawnPlace(waveConfig, rangeTry);
        }
    }
}