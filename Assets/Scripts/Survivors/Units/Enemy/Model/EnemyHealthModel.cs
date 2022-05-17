using Survivors.Units.Enemy.Config;
using Survivors.Units.Model;

namespace Survivors.Units.Enemy.Model
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