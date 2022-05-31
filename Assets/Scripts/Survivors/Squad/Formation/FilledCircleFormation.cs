using UnityEngine;

namespace Survivors.Squad.Formation
{
    public class FilledCircleFormation: ISquadFormation
    {
        private readonly CirclePackingData _packingData;

        public FilledCircleFormation()
        {
            _packingData = Resources.Load<CirclePackingData>("CirclePacking");
        }

        public Vector3 GetUnitOffset(int unitIdx, float unitRadius, int unitsCount)
        {
            var pos = _packingData.GetPos(unitIdx, unitsCount);
            return GetRadius(unitRadius, unitsCount) * new Vector3(pos.X, 0, pos.Y);
        }

        //This algorithm packs circle too well for us
        //So let place unit a bit more scarce when there are a lot of them
        private float GetRadius(float unitRadius, int unitsCount)
        {
            if (unitsCount <= 4) return unitRadius;
            if (unitsCount <= 10) return 1.5f * unitRadius;
            return 2.0f * unitRadius;
        }
    }
}