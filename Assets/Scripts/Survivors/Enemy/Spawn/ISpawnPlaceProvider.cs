using Survivors.Enemy.Spawn.Config;
using UnityEngine;

namespace Survivors.Enemy.Spawn
{
    public interface ISpawnPlaceProvider
    {
        SpawnPlace GetSpawnPlace(EnemyWaveConfig waveConfig, int rangeTry);
    }
}