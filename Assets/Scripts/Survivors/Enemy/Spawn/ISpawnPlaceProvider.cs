using Survivors.Enemy.Spawn.Config;
using UnityEngine;

namespace Survivors.Enemy.Spawn
{
    public interface ISpawnPlaceProvider
    {
        Vector3 GetSpawnPlace(EnemyWaveConfig waveConfig, int outOfViewMultiplier);
    }
}