﻿using Survivors.WorldEvents.Spawner;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Survivors.WorldEvents.Events.Lava.Config
{
    [CreateAssetMenu(fileName = "LavaEventConfig", menuName = "ScriptableObjects/EventConfig/LavaEventConfig")]
    public class LavaEventConfig : EventConfig
    {
        [SerializeField]
        private CircleSpawnParams _spawnParams;
        [SerializeField]
        private float _minLavaRadius = 4;
        [SerializeField]
        private float _maxLavaRadius = 8;
        [SerializeField]
        private float _damagePeriod = 1;
        [SerializeField]
        private float _damagePercent = 5;
        [SerializeField]
        private string _lavaPrefabId = "Lava";
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
        public CircleSpawnParams SpawnParams => _spawnParams;
        
    }
}