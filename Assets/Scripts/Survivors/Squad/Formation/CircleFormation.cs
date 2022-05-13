using UnityEngine;

namespace Survivors.Squad.Formation
{
    public class CircleFormation: ISquadFormation
    {
        public Vector3 GetUnitOffset(int unitIdx, float unitSize, int squadSize)
        {
            var radius = squadSize == 1 ? 0 : squadSize * unitSize / Mathf.PI / 2;
            var angle = 360 * unitIdx / squadSize;
            return Quaternion.AngleAxis(angle, Vector3.up) * Vector3.right * radius;
        }

        public Vector3 GetSpawnOffset(float unitSize, int squadSize)
        {
            return squadSize == 1 ? GetUnitOffset(1, unitSize, 2) : Vector3.zero;
        }
    }
}