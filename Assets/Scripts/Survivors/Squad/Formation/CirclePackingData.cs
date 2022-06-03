using System;
using System.Collections.Generic;
using UnityEngine;

namespace Survivors.Squad.Formation
{

    public class CirclePackingData: ScriptableObject
    {
        [Serializable]
        public struct Position
        {
            public float X;
            public float Y;
        }    
        
        [Serializable]
        public struct CirclePacking
        {
            public List<Position> Positions;
        }
        
        [SerializeField]
        private List<CirclePacking> _packings;

        public Position GetPosition(int positionIdx, int totalPositionCount)
        {
            return _packings[totalPositionCount - 1].Positions[positionIdx];;
        }

        public void SetData(List<CirclePacking> packings)
        {
            _packings = packings;
        }
    }
}