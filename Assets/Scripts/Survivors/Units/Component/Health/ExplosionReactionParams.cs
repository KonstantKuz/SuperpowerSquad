using System;
using UnityEngine;

namespace Survivors.Units.Component.Health
{
    [Serializable]
    public struct ExplosionReactionParams
    {
        public float JumpDistance;
        public float JumpHeight;
        public float JumpDuration;
        
        [HideInInspector]
        public Vector3 ExplosionPosition;
    }
}