using System;

namespace Survivors.Units
{
    public enum UnitType
    {
        ENEMY,
        PLAYER
    }

    public static class UnitTypeExtension
    {
        public static UnitType GetTargetUnitType(this UnitType unitType)
        {
            return unitType switch {
                    UnitType.ENEMY => UnitType.PLAYER,
                    UnitType.PLAYER => UnitType.ENEMY,
                    _ => throw new ArgumentOutOfRangeException(nameof(unitType), unitType, null)
            };
        }
    }
}