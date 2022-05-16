using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Survivors.Location
{
    public class GameWorld : MonoBehaviour
    {
        public List<GameObject> GetSceneObjects()
        {
            return GetComponentsInChildren<Transform>(true).ToList().Select(t => t.gameObject).ToList();
        }
        
        public List<T> GetObjectComponents<T>()
        {
            return GetSceneObjects()
                   .Where(go => go.GetComponent<T>() != null)
                   .Select(go => go.GetComponent<T>())
                   .ToList();
        }

    }
}