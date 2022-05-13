using Survivors.Location;
using Survivors.Location.Service;
using Survivors.Units.Config;
using Survivors.Units.Enemy;
using Survivors.Units.Model;
using UnityEngine;
using Zenject;

namespace Survivors.Units.Service
{
    public class UnitFactory
    {
        private const string SIMPLE_ENEMY_ID = "SimpleEnemy";

        [Inject] private EnemyUnitConfigs _enemyUnitConfigs;
        [Inject] private LocationWorld _locationWorld;
        [Inject] private LocationObjectFactory _locationObjectFactory;
        
        public GameObject CreateEnemy()
        {
            var enemy =_locationObjectFactory.CreateObject(SIMPLE_ENEMY_ID, _locationWorld.SpawnContainer);
            var config = _enemyUnitConfigs.GetConfig(SIMPLE_ENEMY_ID);
            var health = new EnemyHealthModel()
            {
                MaxHealth = config.Health, 
                StartingHealth = config.Health,
            };
            enemy.GetComponent<EnemyAi>().Init(health);
            return enemy;
        }
    }
}