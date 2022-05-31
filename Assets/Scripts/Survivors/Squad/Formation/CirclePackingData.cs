using System;
using System.Collections.Generic;
using UnityEngine;

namespace Survivors.Squad.Formation
{
    [Serializable]
    public struct Pos
    {
        public float X;
        public float Y;
    }
    public class CirclePackingData: ScriptableObject
    {
        public List<Pos> Packings;

        public Pos GetPos(int idx, int count)
        {
            return Packings[idx + (count - 1) * count / 2];
        }
    }
}