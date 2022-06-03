using UnityEngine;

namespace Survivors.Squad.Formation
{
    public class FilledCircleFormation: ISquadFormation
    {
        public const string CIRCLE_PACKING_ASSET_NAME = "CirclePacking";
        private const int MEDIUM_SQUAD_SIZE = 5;
        private const int BIG_SQUAD_SIZE = 11;
        
        private const float SMALL_SQUAD_SIZE_MULTIPLIER = 1f;
        private const float MEDIUM_SQUAD_SIZE_MULTIPLIER = 1.5f;
        private const float BIG_SQUAD_SIZE_MULTIPLIER = 2.0f;
        private readonly CirclePackingData _packingData;

        public FilledCircleFormation()
        {
            _packingData = Resources.Load<CirclePackingData>(CIRCLE_PACKING_ASSET_NAME);
        }

        public Vector3 GetUnitOffset(int unitIdx, float unitRadius, int unitsCount)
        {
            var pos = _packingData.GetPosition(unitIdx, unitsCount);
            return GetFormationScale(unitRadius, unitsCount) * new Vector3(pos.X, 0, pos.Y);
        }

        //This algorithm packs circle too well for us
        //So let place unit a bit more scarce when there are a lot of them
        private float GetFormationScale(float unitRadius, int unitsCount) => unitRadius * GetRadiusMultiplier(unitsCount);
        
        private float GetRadiusMultiplier(int unitsCount)
        {
            if (unitsCount < MEDIUM_SQUAD_SIZE) return SMALL_SQUAD_SIZE_MULTIPLIER;
            if (unitsCount < BIG_SQUAD_SIZE) return MEDIUM_SQUAD_SIZE_MULTIPLIER;
            return BIG_SQUAD_SIZE_MULTIPLIER;
        }
    }
}