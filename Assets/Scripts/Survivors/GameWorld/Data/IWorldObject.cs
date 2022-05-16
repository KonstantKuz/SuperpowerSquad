using UnityEngine;

namespace Survivors.GameWorld.Data
{
    public interface IWorldObject 
    {
        string ObjectId { get; }
        GameObject GameObject { get; }
    }
}