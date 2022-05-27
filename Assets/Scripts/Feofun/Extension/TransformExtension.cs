using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Feofun.Extension
{
    public static class TransformExtension
    {
        public static IEnumerable<Transform> Children(this Transform transform)
        {
            for (int i = 0; i < transform.childCount; i++) {
                yield return transform.GetChild(i);
            }
        }

        public static void DestroyAllChildren(this Transform transform)
        {
            for (var i = transform.childCount - 1; i >= 0; i--) {
                var child = transform.GetChild(i);
                if (!child.gameObject) continue;
                Object.Destroy(child.gameObject);
            }
        }

        public static void ResetLocalTransform(this Transform transform)
        {
            transform.localPosition = Vector3.one;
            transform.localScale = Vector3.one;
            transform.localRotation = Quaternion.identity;
        }

        public static Vector3 WorldToScreenPoint(this Transform transform) => Camera.main.WorldToScreenPoint(transform.position);
    }
}