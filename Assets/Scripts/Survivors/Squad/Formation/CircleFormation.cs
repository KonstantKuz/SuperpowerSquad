using UnityEngine;

namespace Survivors.Squad.Formation
{
    public class CircleFormation: ISquadFormation
    {
        public Vector3 GetUnitOffset(int unitIdx, float unitRadius, int unitsCount)
        {
            var formationRadius = unitsCount == 1 ? 0 : unitsCount * unitRadius / Mathf.PI / 2;
            var angle = 360 * unitIdx / unitsCount;
            return Quaternion.AngleAxis(angle, Vector3.up) * Vector3.right * formationRadius;
        }

        public Vector3 GetSpawnOffset(float unitRadius, int unitCountBefore)
        {
            const int singleUnitSquad = 1;
            var isUnitInCenter = unitCountBefore == singleUnitSquad; //the only squad size when there is a unit right in center
            return isUnitInCenter ? GetUnitOffset(singleUnitSquad, unitRadius, singleUnitSquad + 1) : Vector3.zero;
        }
    }
}