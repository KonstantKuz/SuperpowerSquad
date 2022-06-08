using System;
using Feofun.Config;
using Survivors.Player.Config;

namespace Survivors.Player.Progress
{
    public class PlayerProgress
    {
        private const int DEFAULT_LEVEL = 1;

        public int Level { get; private set; }
        public int Kills { get; private set; }

        private PlayerProgress()
        {
            Level = DEFAULT_LEVEL;
            Kills = 0;
        }
        public static PlayerProgress Create() => new PlayerProgress();

        public void ResetKills()
        {
            Kills = 0;
        }
        public bool IsMaxLevel(StringKeyedConfigCollection<PlayerLevelConfig> levels) => Level > levels.Values.Count;
        public int MaxKillsForCurrentLevel(StringKeyedConfigCollection<PlayerLevelConfig> levels) => CurrentLevelConfig(levels).KillCountToNextLevel;

        public PlayerLevelConfig CurrentLevelConfig(StringKeyedConfigCollection<PlayerLevelConfig> levels) =>
                levels.Values[Math.Min(levels.Values.Count - 1, Level - 1)];

        public void AddKill(StringKeyedConfigCollection<PlayerLevelConfig> levels)
        {
            Kills++;
            while (Kills >= MaxKillsForCurrentLevel(levels) && !IsMaxLevel(levels)) {
                Kills -= MaxKillsForCurrentLevel(levels);
                Level++;
            }
        }
    }
}