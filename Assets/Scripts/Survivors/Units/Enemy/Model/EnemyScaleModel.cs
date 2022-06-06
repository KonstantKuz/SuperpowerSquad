using Survivors.Units.Enemy.Config;

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

        public float GetScaleFactor(int level) => _config.GetScaleForLevel(level);
    }
}