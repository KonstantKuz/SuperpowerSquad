using JetBrains.Annotations;
using UnityEngine;

namespace Survivors.ObjectPool.Wrapper
{
    public interface IObjectPoolWrapper
    {
        IObjectPool<GameObject> BuildObjectPool(GameObject prefab, [CanBeNull] ObjectPoolParams poolParams = null);
    }
}