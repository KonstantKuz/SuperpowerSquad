using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Survivors.Location
{
    public class RootContainer : MonoBehaviour
    {
        public List<T> GetChildrenSubscribers<T>()
        {
            return GetChildrenObjects().Where(go => go.GetComponent<T>() != null).Select(go => go.GetComponent<T>()).ToList();
        }

        public List<GameObject> GetChildrenObjects()
        {
            return GetComponentsInChildren<Transform>(true).Select(it => it.gameObject).ToList();
        }
    }
}