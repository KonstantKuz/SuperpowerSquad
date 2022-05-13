using UnityEngine;

namespace Survivors.Squad.Formation
{
    public interface ISquadFormation
    {
        Vector3 GetUnitOffset(int unitIdx, float unitSize, int squadSize);
        Vector3 GetSpawnOffset(float unitSize, int squadSize);
    }
}