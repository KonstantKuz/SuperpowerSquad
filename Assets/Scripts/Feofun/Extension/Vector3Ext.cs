using UnityEngine;

namespace Feofun.Extension
{
    public static class Vector3Ext
    {
        public static float DistanceXZ(Vector3 a, Vector3 b) => Mathf.Sqrt((a.x - b.x) * (a.x - b.x) + (a.z - b.z) * (a.z - b.z));

        public static Vector3 XZ(this Vector3 v) => new Vector3(v.x, 0, v.z);
        
        public static Vector2 ToVector2XZ(this Vector3 v) => new Vector2(v.x, v.z);
    }
}