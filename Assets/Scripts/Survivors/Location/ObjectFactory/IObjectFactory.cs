using JetBrains.Annotations;
using UnityEngine;

namespace Survivors.Location.ObjectFactory
{
    public interface IObjectFactory
    {
        T Create<T>(string objectId, [CanBeNull] Transform container = null)
                where T : MonoBehaviour;

        T Create<T>(GameObject prefab, [CanBeNull] Transform container = null)
            where T : MonoBehaviour;

        void Destroy<T>(GameObject item)
                where T : MonoBehaviour;

        void DestroyAllObjects();
    }
}