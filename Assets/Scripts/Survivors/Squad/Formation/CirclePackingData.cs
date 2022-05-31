using System;
using System.Collections.Generic;
using System.Linq;
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
        
        [SerializeField]
        private List<Pos> _positions;

        public Pos GetPos(int idx, int count)
        {
            return _positions[idx + (count - 1) * count / 2];
        }

        public void SetData(Dictionary<int, List<Pos>> data)
        {
            _positions = data.OrderBy(it => it.Key).SelectMany(it => it.Value).ToList();
        }
    }
}