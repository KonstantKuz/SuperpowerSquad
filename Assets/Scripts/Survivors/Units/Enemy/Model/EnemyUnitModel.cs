using Survivors.Units.Enemy.Config;
using Survivors.Units.Model;

namespace Survivors.Units.Enemy.Model
{
    public class EnemyUnitModel : IUnitModel
    {
        public string Id { get; set; }
        public IUnitHealthModel HealthModel { get; set; }
        public IAttackModel AttackModel { get; }

        public EnemyUnitModel(EnemyUnitConfig config)
        {
            Id = config.Id;
            HealthModel = new HealthModel(config.Health);
        }
    }
}