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

        private EnemyUnitConfig _config;
        public string Id { get; }
        public float MoveSpeed { get; }
        public int Level { get; }
        public IHealthModel HealthModel { get; }
        public IAttackModel AttackModel { get; }
        public EnemyScaleModel ScaleModel { get; }
        
        public EnemyUnitModel(EnemyUnitConfig config, int level = 1)
        {
            Assert.IsTrue(level >= MIN_LEVEL);
            _config = config;
            Id = config.Id;
            MoveSpeed = config.MoveSpeed;
            Level = level;
            ScaleModel = new EnemyScaleModel(config, level);
            HealthModel = new EnemyHealthModel(config.Health + (level - MIN_LEVEL) * config.HealthStep);
            AttackModel = new EnemyAttackModel(config.EnemyAttackConfig);
        }

        public int CalculateLevelOfHealth(float currentHealth)
        {
            return currentHealth <= _config.Health ? MIN_LEVEL : MIN_LEVEL + (int) Mathf.Ceil((currentHealth - _config.Health) / _config.HealthStep);
        }

        public void AddModifier(IModifier modifier)
        {
            //do nothing
        }
    }
}