using UnityEngine;

namespace Survivors.Location.Data
{
    public interface IWorldObject 
    {
        string ObjectId { get; }
        GameObject GameObject { get; }
    }
}