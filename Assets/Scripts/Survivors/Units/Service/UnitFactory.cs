using Survivors.Location;
using Survivors.Location.Service;
using UnityEngine;
using Zenject;

namespace Survivors.Units.Service
{
    public class UnitFactory
    {
        private const string SIMPLE_ENEMY_ID = "SimpleEnemy";
        
        [Inject] private World _world;
        [Inject] private WorldObjectFactory _worldObjectFactory;
        
        public GameObject CreateEnemy()
        {
            return _worldObjectFactory.CreateObject(SIMPLE_ENEMY_ID, _world.SpawnContainer);
        }
    }
}