using System;
using System.Collections.Generic;
using UnityEngine;

namespace Survivors.Squad.Formation
{

    public class CirclePackingData: ScriptableObject
    {
        [Serializable]
        public struct Pos
        {
            public float X;
            public float Y;
        }    
        
        [Serializable]
        public struct CirclePacking
        {
            public List<Pos> Positions;
        }
        
        [SerializeField]
        private List<CirclePacking> _packings;

        public Pos GetPos(int idx, int count)
        {
            return _packings[count - 1].Positions[idx];;
        }

        public void SetData(List<CirclePacking> packings)
        {
            _packings = packings;
        }
    }
}