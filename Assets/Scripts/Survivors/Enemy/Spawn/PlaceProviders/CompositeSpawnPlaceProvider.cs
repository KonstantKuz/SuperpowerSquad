using Survivors.Enemy.Spawn.Config;
using Survivors.Enemy.Spawn.Spawners;
using Survivors.Location;
using Random = UnityEngine.Random;

namespace Survivors.Enemy.Spawn.PlaceProviders
{
    public class CompositeSpawnPlaceProvider : ISpawnPlaceProvider
    {
        private readonly ISpawnPlaceProvider _randomDrivenProvider;

        public CompositeSpawnPlaceProvider(EnemyWaveSpawner spawner, World world)
        {
            _randomDrivenProvider = new RandomSideDrivenPlaceProvider(spawner, world);
        }
        
        public SpawnPlace GetSpawnPlace(EnemyWaveConfig waveConfig, float outOfViewOffset)
        {
            return _randomDrivenProvider.GetSpawnPlace(waveConfig, outOfViewOffset);
        }
    }
}