using System;
using JetBrains.Annotations;
using UnityEngine;

namespace Survivors.ObjectPool.Wrapper
{
    public interface IObjectPoolWrapper
    {
        IObjectPool<GameObject> BuildObjectPool(GameObject prefab, 
            Action<GameObject> onObjectCreated, 
            [CanBeNull] ObjectPoolParams poolParams = null);
    }
}