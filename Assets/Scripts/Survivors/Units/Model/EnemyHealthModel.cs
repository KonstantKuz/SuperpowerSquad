using System;
using Survivors.Units.Config;

namespace Survivors.Units.Model
{
    public class EnemyHealthModel : IUnitHealthModel
    {
        public int MaxHealth { get; }
        public int StartingHealth { get; }

        public EnemyHealthModel(EnemyUnitConfig config)
        {
            MaxHealth = config.Health; 
            StartingHealth = config.Health;
        }
    }
}