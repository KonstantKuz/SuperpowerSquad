using UnityEngine;

namespace Survivors.Squad.Formation
{
    public class FilledCircleFormation: ISquadFormation
    {
        private CirclePackingData _packingData;

        public FilledCircleFormation()
        {
            _packingData = Resources.Load<CirclePackingData>("CirclePacking");
        }

        public Vector3 GetUnitOffset(int unitIdx, float unitRadius, int unitsCount)
        {
            var pos = _packingData.GetPos(unitIdx, unitsCount);
            var radius = unitRadius;
            if (unitsCount > 5)
            {
                radius = 1.5f * unitRadius;
            }

            if (unitsCount > 11)
            {
                radius = 2.0f * unitRadius;
            }
            return radius * new Vector3(pos.X, 0, pos.Y);
        }
    }
}