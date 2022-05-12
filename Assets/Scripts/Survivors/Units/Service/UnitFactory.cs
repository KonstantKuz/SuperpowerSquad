using Survivors.Location;
using Survivors.Location.Service;
using UnityEngine;
using Zenject;

namespace Survivors.Units.Service
{
    public class UnitFactory
    {
        private const string SIMPLE_ENEMY_ID = "SimpleEnemy";
        
        [Inject] private LocationWorld _locationWorld;
        [Inject] private LocationObjectFactory _locationObjectFactory;
        
        public GameObject CreateEnemy()
        {
            return _locationObjectFactory.CreateObject(SIMPLE_ENEMY_ID, _locationWorld.SpawnContainer);
        }
    }
}