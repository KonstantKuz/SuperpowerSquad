using System;
using Survivors.Squad.Data;

namespace Survivors.Loot
{
    public enum DroppingLootType
    {
        Exp,
        Token
    }
    public static class DroppingLootTypeExtension
    {
        public static SquadProgressType ToSquadProgressType(this DroppingLootType droppingLootType)
        {
            return droppingLootType switch
            {
                DroppingLootType.Exp => SquadProgressType.Exp,
                DroppingLootType.Token => SquadProgressType.Token,
                _ => throw new ArgumentOutOfRangeException(
                    $"Invalid convert from droppingLootType:= {droppingLootType} to squadProgressType")
            };
        }
    }
}