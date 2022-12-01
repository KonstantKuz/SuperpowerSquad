using System.Linq;
using Feofun.Extension;
using Survivors.Enemy.Spawn.Config;
using Survivors.Enemy.Spawn.Spawners;
using Survivors.Location;

namespace Survivors.Enemy.Spawn.PlaceProviders
{
    public class SequentialSpawnPlaceProvider : ISpawnPlaceProvider
    {
        private readonly EnemyWaveSpawner _spawner;
        private readonly RandomSideDrivenPlaceProvider _randomDrivenProvider;

        private int _spawnSide;
        public SequentialSpawnPlaceProvider(EnemyWaveSpawner spawner, World world)
        {
            _spawner = spawner;
            _randomDrivenProvider = new RandomSideDrivenPlaceProvider(spawner, world);
        }
 
        public SpawnPlace GetSpawnPlace(EnemyWaveConfig waveConfig, float outOfViewOffset)
        {
            _spawnSide++;
            _spawnSide %= EnumExt.Values<SpawnSide>().Count();
            
            var position = _randomDrivenProvider.GetRandomSpawnPosition((SpawnSide) _spawnSide, outOfViewOffset);
            var isValid = _spawner.IsPlaceValid(position, waveConfig);
            return new SpawnPlace {
                IsValid = isValid,
                Position = position
            };
        }
    }
}