using System;
using UnityEngine;

namespace Survivors.WorldEvents.Spawner
{
    [Serializable]
    public class CircleSpawnParams : ICircleSpawnParams
    {
        [SerializeField]
        private int _initialSpawnCountOnCircle = 5;
        [SerializeField]
        private int _spawnCountIncrementStepOnCircle = 6;
        [SerializeField]
        private float _initialSpawnDistance = 8;
        [SerializeField]
        private float _spawnDistanceStep = 16;
        public int InitialSpawnCountOnCircle => _initialSpawnCountOnCircle;
        public int SpawnCountIncrementStepOnCircle => _spawnCountIncrementStepOnCircle;
        public float InitialSpawnDistance => _initialSpawnDistance;
        public float SpawnDistanceStep => _spawnDistanceStep;
        public float MaxSpawnDistance { get; set; }
    }
}