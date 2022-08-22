using System;
using UnityEngine;

namespace Survivors.WorldEvents.Spawner
{
    [Serializable]
    public class CircleSpawnParams : ICircleSpawnParams
    {
        [SerializeField]
        private int _initialSpawnCount = 5;
        [SerializeField]
        private int _spawnCountIncrementStep = 6;
        [SerializeField]
        private float _initialSpawnDistance = 8;
        [SerializeField]
        private float _spawnDistanceStep = 16;
        public int InitialSpawnCount => _initialSpawnCount;
        public int SpawnCountIncrementStep => _spawnCountIncrementStep;
        public float InitialSpawnDistance => _initialSpawnDistance;
        public float SpawnDistanceStep => _spawnDistanceStep;
        public float MaxSpawnDistance { get; set; }
    }
}