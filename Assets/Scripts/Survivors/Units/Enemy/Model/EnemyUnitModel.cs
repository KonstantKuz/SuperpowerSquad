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
        public float ScaleFactor { get; }
        public EnemyUnitConfig Config { get; }

        public EnemyUnitModel(EnemyUnitConfig config, int level = 1)
        {
            Assert.IsTrue(level >= MIN_LEVEL);
            Config = config;
            Id = config.Id;
            MoveSpeed = config.MoveSpeed;
            Level = level;
            ScaleFactor = CalculateScaleFactor(level);
            HealthModel = new EnemyHealthModel(config.Health + (level - MIN_LEVEL) * config.HealthStep);
            AttackModel = new EnemyAttackModel(config.EnemyAttackConfig);
        }

        public float CalculateScaleFactor(int level)
        {
            Assert.IsTrue(level >= MIN_LEVEL); 
            return Mathf.Pow(Config.ScaleStep, level - MIN_LEVEL);
        }

        public void AddModifier(IModifier modifier)
        {
            //do nothing
        }
    }
}