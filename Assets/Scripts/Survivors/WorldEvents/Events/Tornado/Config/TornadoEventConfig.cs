using UnityEngine;

namespace Survivors.WorldEvents.Events.Tornado.Config
{
    [CreateAssetMenu(fileName = "TornadoEventConfig", menuName = "ScriptableObjects/EventConfig/TornadoEventConfig")]
    public class TornadoEventConfig : EventConfig
    {
        [SerializeField]
        private int _spawnCountOnCircle = 5;
        [SerializeField]
        public int _spawnCountStepOnCircle = 6;
    
        [SerializeField]
        public float _minLavaRadius = 4;
        [SerializeField]
        public float _maxLavaRadius = 8;
        
        
        [SerializeField]
        private string _prefabId = "Lava";
        
        [SerializeField]
        private float _minAppearTime = 3;
        [SerializeField]
        private float _maxAppearTime = 7;
        [SerializeField]
        private float _minDisappearTime = 3;
        [SerializeField]
        private float _maxDisappearTime = 7;
        
        public int SpawnCountOnCircle => _spawnCountOnCircle;
        
        public int SpawnCountStepOnCircle => _spawnCountStepOnCircle;
        
        public string PrefabId => _prefabId;
        
        
        public float RandomAppearTime => Random.Range(_minAppearTime, _maxAppearTime); 
        public float RandomDisappearTime => Random.Range(_minDisappearTime, _maxDisappearTime);
        
    }
}