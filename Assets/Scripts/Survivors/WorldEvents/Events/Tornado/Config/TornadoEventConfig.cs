using Survivors.WorldEvents.Spawner;
using UnityEngine;

namespace Survivors.WorldEvents.Events.Tornado.Config
{
    [CreateAssetMenu(fileName = "TornadoEventConfig", menuName = "ScriptableObjects/EventConfig/TornadoEventConfig")]
    public class TornadoEventConfig : EventConfig
    {
        [SerializeField]
        private CircleSpawnParams _spawnParams;
        [SerializeField]
        private string _prefabId = "Tornado";
        
        [SerializeField]
        private float _minAppearTime = 3;
        [SerializeField]
        private float _maxAppearTime = 7;
        [SerializeField]
        private float _minDisappearTime = 3;
        [SerializeField]
        private float _maxDisappearTime = 7; 
        
        [SerializeField]
        private float _speed = 1.5f;
        [SerializeField]
        private float _directionChangeTimeout = 1;  
        [SerializeField]
        private float _maxRandomAngle = 90;
        
        public string PrefabId => _prefabId;     
        public CircleSpawnParams SpawnParams => _spawnParams;
        public float RandomAppearTime => Random.Range(_minAppearTime, _maxAppearTime); 
        public float RandomDisappearTime => Random.Range(_minDisappearTime, _maxDisappearTime);
        
        public float Speed => _speed;
        public float DirectionChangeTimeout => _directionChangeTimeout;
        public float MaxRandomAngle => _maxRandomAngle;
    }
}