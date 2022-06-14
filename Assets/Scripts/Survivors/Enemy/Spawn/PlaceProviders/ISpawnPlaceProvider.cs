using Survivors.Enemy.Spawn.Config;

namespace Survivors.Enemy.Spawn.PlaceProviders
{
    public interface ISpawnPlaceProvider
    {
        SpawnPlace GetSpawnPlace(EnemyWaveConfig waveConfig, int rangeTry);
    }
}