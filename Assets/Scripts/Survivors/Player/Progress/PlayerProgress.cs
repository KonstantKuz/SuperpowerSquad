using System;
using Feofun.Config;
using Survivors.Enemy.Config;

namespace Survivors.Player.Progress
{
    public class PlayerProgress
    {
        private const int DEFAULT_LEVEL = 1;

        public int WinCount { get; private set; }


        private PlayerProgress()
        {
            WinCount = DEFAULT_LEVEL;
            Kills = 0;
        }
        public static PlayerProgress Create() => new PlayerProgress();
        
        public bool IsMaxLevel(StringKeyedConfigCollection<EnemyLevelConfig> levels) => WinCount > levels.Values.Count;
        public int MaxKillsForCurrentLevel(StringKeyedConfigCollection<EnemyLevelConfig> levels) => CurrentLevelConfig(levels).KillCount;

        public EnemyLevelConfig CurrentLevelConfig(StringKeyedConfigCollection<EnemyLevelConfig> levels) =>
                levels.Values[Math.Min(levels.Values.Count - 1, WinCount - 1)];

        public void AddKill(StringKeyedConfigCollection<EnemyLevelConfig> levels)
        {
            Kills++;
            while (Kills >= MaxKillsForCurrentLevel(levels) && !IsMaxLevel(levels)) {
                Kills -= MaxKillsForCurrentLevel(levels);
                WinCount++;
            }
        }
    }
}