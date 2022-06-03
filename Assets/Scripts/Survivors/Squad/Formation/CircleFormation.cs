using UnityEngine;

namespace Survivors.Squad.Formation
{
    public class CircleFormation: ISquadFormation
    {
        private const int SINGLE_UNIT_SQUAD = 1;        
        
        public Vector3 GetUnitOffset(int unitIdx, float unitRadius, int unitsCount)
        {
            if (unitsCount == SINGLE_UNIT_SQUAD) return Vector3.zero;
            var formationRadius = unitsCount * unitRadius / Mathf.PI / 2;
            var angle = 360 * unitIdx / unitsCount;
            return Quaternion.AngleAxis(angle, Vector3.up) * Vector3.right * formationRadius;
        }
    }
}