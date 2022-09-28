using ModestTree;
using UnityEngine;

namespace Feofun.Extension
{
    public static class GameObjectExt
    {
        public static T RequireComponentInParent<T>(this GameObject gameObject)
        {
            var component = gameObject.GetComponentInParent<T>();
            Assert.IsNotNull(component, $"{gameObject.name} gameObject is missing {typeof(T).Name} component in hierarchy");
            return component;
        }
        public static T RequireComponentInChildren<T>(this GameObject gameObject)
        {
            var component = gameObject.GetComponentInChildren<T>();
            Assert.IsNotNull(component, $"{gameObject.name} gameObject is missing {typeof(T).Name} component in hierarchy");
            return component;
        } 
        public static T RequireComponent<T>(this GameObject gameObject)
        {
            var component = gameObject.GetComponent<T>();
            Assert.IsNotNull(component, $"{gameObject.name} gameObject is missing {typeof(T).Name} component in hierarchy");
            return component;
        }
        public static bool IsInViewport(this Vector3 position)
        {
            var viewportPosition = Camera.main.WorldToViewportPoint(position);
            return viewportPosition.x >= 0 && viewportPosition.x <= 1 && 
                   viewportPosition.y >= 0 && viewportPosition.y <= 1 && 
                   viewportPosition.z > 0;
        }
    }
}