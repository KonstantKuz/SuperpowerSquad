using Survivors.Units.Enemy.Config;
using UnityEngine;
using UnityEngine.Assertions;

namespace Survivors.Units.Enemy.Model
{
    public class EnemyScaleModel
    {
        private EnemyUnitConfig _config;
        public float InitialScaleFactor { get; }
        
        public EnemyScaleModel(EnemyUnitConfig config, int level)
        {
            _config = config;
            InitialScaleFactor = GetScaleFactor(level);
        }
        public float GetScaleFactor(int level)
        {
            Assert.IsTrue(level >= EnemyUnitConfig.MIN_LEVEL);
            return Mathf.Pow(_config.ScaleStep, level - EnemyUnitConfig.MIN_LEVEL);
        }
    }
}