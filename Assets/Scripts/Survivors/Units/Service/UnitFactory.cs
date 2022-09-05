using Feofun.Config;
using ModestTree;
using Survivors.Location;
using Survivors.Location.ObjectFactory;
using Survivors.Units.Enemy.Config;
using Survivors.Units.Enemy.Model;
using Zenject;

namespace Survivors.Units.Service
{
    public class UnitFactory
    {
        [Inject] private World _world;
        [Inject(Id = ObjectFactoryType.Instancing)] 
        private IObjectFactory _objectFactory;
        
        [Inject(Id = ObjectFactoryType.Pool)] 
        private IObjectFactory _poolObjectFactory;  
        
        [Inject] private StringKeyedConfigCollection<EnemyUnitConfig> _enemyUnitConfigs;
        [Inject] private PlayerUnitModelBuilder _playerUnitModelBuilder;
        
        public void CreatePlayerUnits(string unitId, int count)
        {
            for (int i = 0; i < count; i++) {
                CreatePlayerUnit(unitId);
            }
        }

        private void CheckSquad() => Assert.IsNotNull(_world.Squad, "Squad is null, should call this method only inside game session");

        public Unit CreatePlayerUnit(string unitId)
        {
            CheckSquad();
            var unit = _objectFactory.Create<Unit>(unitId);
            var model = _playerUnitModelBuilder.BuildUnit(unitId);
            unit.Init(model);
            _world.Squad.AddUnit(unit);
            return unit;
        }
        
        public Unit CreateEnemy(string unitId, int level)
        {
            var enemy = _poolObjectFactory.Create<Unit>(unitId, _world.Spawn.transform);
            var config = _enemyUnitConfigs.Get(unitId);
            var model = new EnemyUnitModel(config, level);
            enemy.Init(model);
            return enemy;
        }
    }
}