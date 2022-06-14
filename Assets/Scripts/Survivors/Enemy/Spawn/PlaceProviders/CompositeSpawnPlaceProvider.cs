using Survivors.Enemy.Spawn.Config;
using Survivors.Location;
using UnityEngine;

namespace Survivors.Enemy.Spawn.PlaceProviders
{
    public class CompositeSpawnPlaceProvider : ISpawnPlaceProvider
    {
        private readonly EnemyWavesSpawner _wavesSpawner;
        private readonly ISpawnPlaceProvider _randomDrivenProvider;
        private readonly ISpawnPlaceProvider _moveDirectionDrivenProvider;

        public CompositeSpawnPlaceProvider(EnemyWavesSpawner wavesSpawner, World world)
        {
            _wavesSpawner = wavesSpawner;
            _randomDrivenProvider = new RandomDrivenPlaceProvider(wavesSpawner, world);
            _moveDirectionDrivenProvider = new MoveDirectionDrivenPlaceProvider(wavesSpawner, world.Squad);
        }
        
        public SpawnPlace GetSpawnPlace(EnemyWaveConfig waveConfig, int rangeTry)
        {
            var placeProvider = Random.value < _wavesSpawner.MoveDirectionDrivenPlaceChance ?
                _moveDirectionDrivenProvider : _randomDrivenProvider;

            return placeProvider.GetSpawnPlace(waveConfig, rangeTry);
        }
    }
}