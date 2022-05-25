﻿using UnityEngine;

namespace Survivors.Squad.Formation
{
    public interface ISquadFormation
    {
        float GetSize(float unitRadius, int unitsCount);
        Vector3 GetUnitOffset(int unitIdx, float unitRadius, int unitsCount);
        Vector3 GetSpawnOffset(float unitRadius, int unitCountBefore);
    }
}