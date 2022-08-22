using System;
using UnityEngine;

namespace Survivors.WorldEvents.Spawner
{
    [Serializable]
    public class CircleSpawnParams : ICircleSpawnParams
    {
        [SerializeField]
        private float _spawnStepOnPerimeter = 10;
        [SerializeField]
        private float _initialSpawnDistance = 8;
        [SerializeField]
        private float _spawnDistanceStep = 16;
        
        public float SpawnStepOnPerimeter => _spawnStepOnPerimeter;
        public float InitialSpawnDistance => _initialSpawnDistance;
        public float SpawnDistanceStep => _spawnDistanceStep;
        public float MaxSpawnDistance { get; set; }
    }
}