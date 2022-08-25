using JetBrains.Annotations;
using UnityEngine;

namespace Survivors.Location.Service
{
    public interface IWorldObjectFactory
    {
        T CreateObject<T>(string objectId, [CanBeNull] Transform container = null)
                where T : MonoBehaviour;

        void DestroyObject<T>(GameObject item)
                where T : MonoBehaviour;

        void DestroyAllObjects();
    }
}