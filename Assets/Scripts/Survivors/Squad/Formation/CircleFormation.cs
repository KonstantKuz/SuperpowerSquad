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

        public Vector3 GetSpawnOffset(float unitRadius, int unitCountBefore)
        {
            var isUnitInCenter = unitCountBefore == SINGLE_UNIT_SQUAD; //the only squad size when there is a unit right in center
            return isUnitInCenter ? GetUnitOffset(SINGLE_UNIT_SQUAD, unitRadius, SINGLE_UNIT_SQUAD + 1) : Vector3.zero;
        }
    }
}