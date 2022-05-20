using Feofun.Modifiers;
using Survivors.Units.Enemy.Config;
using Survivors.Units.Model;

namespace Survivors.Units.Enemy.Model
{
    public class EnemyUnitModel : IUnitModel
    {
        public string Id { get; }
        public float MoveSpeed { get; }
        public HealthModel HealthModel { get; }
        public IAttackModel AttackModel { get; }

        public EnemyUnitModel(EnemyUnitConfig config)
        {
            Id = config.Id;
            MoveSpeed = config.MoveSpeed;
            HealthModel = new HealthModel(config.Health);
            AttackModel = new EnemyAttackModel(config.EnemyAttackConfig);
        }

        public void AddModifier(IModifier modifier)
        {
            //do nothing
        }
    }
}