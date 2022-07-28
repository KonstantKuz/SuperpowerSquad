using System.Collections.Generic;

namespace Survivors.Enemy.Spawn.Config
{
    public class LevelWavesConfig
    {
        public readonly int Level;
        public readonly IReadOnlyList<EnemyWaveConfig> Waves;

        public LevelWavesConfig(int level, IReadOnlyList<EnemyWaveConfig> waves)
        {
            Level = level;
            Waves = waves;
        }
    }
}