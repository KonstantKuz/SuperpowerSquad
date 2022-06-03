using Feofun.Modifiers;
using Survivors.Units.Enemy.Config;
using Survivors.Units.Model;
using UnityEngine;
using UnityEngine.Assertions;

namespace Survivors.Units.Enemy.Model
{
    public class EnemyUnitModel : IUnitModel
    {
        public string Id { get; }
        public float MoveSpeed { get; }
        public IHealthModel HealthModel { get; }
        public IAttackModel AttackModel { get; }
        public int Level { get; }
        public float Scale { get; }

        public EnemyUnitModel(EnemyUnitConfig config, int level = 1)
        {
            Assert.IsTrue(level >= EnemyUnitConfig.MIN_LEVEL);
            Id = config.Id;
            MoveSpeed = config.MoveSpeed;
            Level = level;
            Scale = config.GetScaleForLevel(level);
            HealthModel = new EnemyHealthModel(config.GetHealthForLevel(level));
            AttackModel = new EnemyAttackModel(config.EnemyAttackConfig);
        }

        public void AddModifier(IModifier modifier)
        {
            //do nothing
        }
    }
}