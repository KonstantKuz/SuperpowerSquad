using JetBrains.Annotations;
using UnityEngine;

namespace Feofun.Extension
{
    [PublicAPI]
    public static class MonoBehaviourExtension
    {
        public static bool IsDestroyed(this MonoBehaviour target)
        {
            return target == null || target.gameObject == null;
        }
    }
}