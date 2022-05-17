using Feofun.Config;
using Survivors.Location;
using Survivors.Location.Service;
using Survivors.Units.Enemy;
using Survivors.Units.Enemy.Config;
using Survivors.Units.Enemy.Model;
using Survivors.Units.Model;
using Zenject;

namespace Survivors.Units.Service
{
    public class UnitFactory
    {
        private const string SIMPLE_ENEMY_ID = "SimpleEnemy";

        [Inject] private StringKeyedConfigCollection<EnemyUnitConfig> _enemyUnitConfigs;
        [Inject] private World _world;
        [Inject] private WorldObjectFactory _worldObjectFactory;
        
        public EnemyUnit CreateEnemy()
        {
            var enemy =_worldObjectFactory.CreateObject(SIMPLE_ENEMY_ID, _world.SpawnContainer).GetComponent<EnemyUnit>();
            var config = _enemyUnitConfigs.Get(SIMPLE_ENEMY_ID);
            var health = new EnemyHealthModel(config);
            enemy.Init(health);
            return enemy;
        }
    }
}