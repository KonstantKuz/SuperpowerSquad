using ModestTree;
using UnityEngine;

namespace Survivors.Extension
{
    public static class AssertExt
    {
        public static T IsNotNullComponent<T>(this T component, Object prefab) where T : Component
        {
            Assert.IsNotNull(component, $"{prefab.name} prefab is missing {typeof(T).Name} component in hierarchy");
            return (T) component;
        }
    }
}