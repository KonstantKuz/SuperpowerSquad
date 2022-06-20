using Feofun.Modifiers;
using Survivors.Units.Enemy.Config;
using Survivors.Units.Model;
using UnityEngine;
using UnityEngine.Assertions;

namespace Survivors.Units.Enemy.Model
{
    public class EnemyUnitModel : IUnitModel
    {
        private EnemyUnitConfig _config;
        public string Id { get; }
        public float MoveSpeed { get; }
        public int Level { get; }
        public IHealthModel HealthModel { get; }
        public IAttackModel AttackModel { get; }

        public EnemyUnitModel(EnemyUnitConfig config, int level = 1)
        {
            Assert.IsTrue(level >= EnemyUnitConfig.MIN_LEVEL);
            _config = config;
            Id = config.Id;
            MoveSpeed = config.MoveSpeed;
            Level = level;
            HealthModel = new EnemyHealthModel(config.GetHealthForLevel(level));
            AttackModel = new EnemyAttackModel(config.EnemyAttackConfig);
        }
        public int CalculateLevelOfHealth(float currentHealth)
        {
            return currentHealth <= _config.Health ? EnemyUnitConfig.MIN_LEVEL : EnemyUnitConfig.MIN_LEVEL + (int) Mathf.Ceil((currentHealth - _config.Health) / _config.HealthStep);
        }
        public float CalculateScale(int level) => _config.CalculateScale(level);
        public void AddModifier(IModifier modifier)
        {
            //do nothing
        }
    }
}