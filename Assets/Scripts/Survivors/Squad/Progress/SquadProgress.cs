using System;
using Feofun.Config;
using Survivors.Squad.Config;

namespace Survivors.Squad.Progress
{
    public class SquadProgress
    {
        public int Level = 1;
        public int Exp;

        public bool IsMaxLevel(StringKeyedConfigCollection<SquadLevelConfig> levels) => Level > levels.Values.Count;

        public int MaxExpForCurrentLevel(StringKeyedConfigCollection<SquadLevelConfig> levels) => CurrentLevelConfig(levels).ExpToNextLevel;

        public SquadLevelConfig CurrentLevelConfig(StringKeyedConfigCollection<SquadLevelConfig> levels) =>
                levels.Values[Math.Min(levels.Values.Count - 1, Level - 1)];

        public void AddExp(int amount, StringKeyedConfigCollection<SquadLevelConfig> levels)
        {
            Exp += amount;
            while (Exp >= MaxExpForCurrentLevel(levels) && !IsMaxLevel(levels))
            {
                Exp -= MaxExpForCurrentLevel(levels);                
                Level++;
            }
        }
    }
}