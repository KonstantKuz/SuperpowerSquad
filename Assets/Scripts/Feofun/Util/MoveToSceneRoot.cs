using UnityEngine;

namespace Feofun.Util
{
    public class MoveToSceneRoot : MonoBehaviour
    {
        private void Awake()
        {
            transform.SetParent(null);
        }
    }
}