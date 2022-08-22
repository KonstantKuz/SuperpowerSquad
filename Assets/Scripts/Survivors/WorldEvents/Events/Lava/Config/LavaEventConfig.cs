using Survivors.WorldEvents.Spawner;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Survivors.WorldEvents.Events.Lava.Config
{
    [CreateAssetMenu(fileName = "LavaEventConfig", menuName = "ScriptableObjects/EventConfig/LavaEventConfig")]
    public class LavaEventConfig : EventConfig, ICircleSpawnParams
    {
        [SerializeField]
        private int _initialSpawnCount = 5;
        [SerializeField]
        public int _spawnCountIncrementStep = 6;
        [SerializeField]
        public float _minLavaRadius = 4;
        [SerializeField]
        public float _maxLavaRadius = 8;
        [SerializeField]
        public float _damagePeriod = 1;
        [SerializeField]
        public float _damagePercent = 30;
        [SerializeField]
        public string _lavaPrefabId = "Lava";
        
        [SerializeField]
        private float _minAppearTime = 3;
        [SerializeField]
        private float _maxAppearTime = 7;
        [SerializeField]
        private float _minDisappearTime = 3;
        [SerializeField]
        private float _maxDisappearTime = 7;
        
        public float DamagePeriod => _damagePeriod;
        public float DamagePercent => _damagePercent;
        public string LavaPrefabId => _lavaPrefabId;
        
        public float RandomAppearTime => Random.Range(_minAppearTime, _maxAppearTime); 
        public float RandomDisappearTime => Random.Range(_minDisappearTime, _maxDisappearTime);
        public float RandomRadius => Random.Range(_minLavaRadius, _maxLavaRadius);
        
        public int InitialSpawnCount => _initialSpawnCount;
        public int SpawnCountIncrementStep => _spawnCountIncrementStep;
        public float InitialSpawnDistance => _minLavaRadius * 2;
        public float SpawnDistanceStep => InitialSpawnDistance * 2;
        
        public float MaxSpawnDistance { get; set; }
    }
}