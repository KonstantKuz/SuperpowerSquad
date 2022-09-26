using Survivors.Enemy.Spawn.Config;
using Survivors.Enemy.Spawn.Spawners;
using Survivors.Location;
using Random = UnityEngine.Random;

namespace Survivors.Enemy.Spawn.PlaceProviders
{
    public class CompositeSpawnPlaceProvider : ISpawnPlaceProvider
    {
        private const float MOVE_DIRECTION_DRIVEN_CHANCE = 0.5f;
        
        private readonly ISpawnPlaceProvider _randomDrivenProvider;
        private readonly ISpawnPlaceProvider _moveDirectionDrivenProvider;

        public CompositeSpawnPlaceProvider(EnemyWaveSpawner spawner, World world)
        {
            _randomDrivenProvider = new RandomSideDrivenPlaceProvider(spawner, world);
            _moveDirectionDrivenProvider = new MoveDirectionDrivenPlaceProvider(spawner, world.Squad);
        }
        
        public SpawnPlace GetSpawnPlace(EnemyWaveConfig waveConfig, float outOfViewOffset)
        {
            if (Random.value < MOVE_DIRECTION_DRIVEN_CHANCE)
            {
                var spawnPlace = _moveDirectionDrivenProvider.GetSpawnPlace(waveConfig, outOfViewOffset);
                if (spawnPlace.IsValid) return spawnPlace;
            }
            return _randomDrivenProvider.GetSpawnPlace(waveConfig, outOfViewOffset);
        }
    }
}