using Feofun.Modifiers;
using Survivors.Units.Enemy.Config;
using Survivors.Units.Model;
using UnityEngine;
using UnityEngine.Assertions;

namespace Survivors.Units.Enemy.Model
{
    public class EnemyUnitModel : IUnitModel
    {
        public const int MIN_LEVEL = 1;
        
        public string Id { get; }
        public float MoveSpeed { get; }
        public IHealthModel HealthModel { get; }
        public IAttackModel AttackModel { get; }
        public int Level { get; }
        public float Scale { get; }

        public EnemyUnitModel(EnemyUnitConfig config, int level = 1)
        {
            Assert.IsTrue(level >= MIN_LEVEL);
            Id = config.Id;
            MoveSpeed = config.MoveSpeed;
            Level = level;
            Scale = Mathf.Pow(config.ScaleStep, level - MIN_LEVEL);
            HealthModel = new EnemyHealthModel(config.Health + (level - MIN_LEVEL) * config.HealthStep);
            AttackModel = new EnemyAttackModel(config.EnemyAttackConfig);
        }

        public void AddModifier(IModifier modifier)
        {
            //do nothing
        }
    }
}