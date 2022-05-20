
using Survivors.Units.Model;

namespace Survivors.Units.Enemy.Model
{
    public class EnemyHealthModel : IHealthModel
    {
        public float MaxHealth { get; }

        public EnemyHealthModel(int maxHealth)
        {
            MaxHealth = maxHealth;
        }
    }
}