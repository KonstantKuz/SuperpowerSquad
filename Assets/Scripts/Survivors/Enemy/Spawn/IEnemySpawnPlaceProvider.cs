using Survivors.Enemy.Spawn.Config;
using UnityEngine;

namespace Survivors.Enemy.Spawn
{
    public interface IEnemySpawnPlaceProvider
    {
        Vector3 GetSpawnPlace(EnemyWaveConfig waveConfig, int spawnOffsetMultiplier);
    }
}