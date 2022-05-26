using System.Collections.Generic;
using System.Linq;
using Feofun.App;
using SuperMaxim.Core.Extensions;
using Survivors.Session;
using UnityEngine;

namespace Survivors.Location
{
    public class World : MonoBehaviour
    {
        [SerializeField]
        private Transform _ground;
        [SerializeField]
        private GameObject _spawnContainer;
        [SerializeField]
        private Squad.Squad _squad;

        public Transform Ground => _ground;
        public GameObject SpawnContainer => _spawnContainer;
        public Squad.Squad Squad => _squad;

        public Vector3 GetGroundIntersection(Ray withRay)
        {
            var plane = new Plane(Ground.up, Ground.position);
            plane.Raycast(withRay, out var intersectionDist);
            return withRay.GetPoint(intersectionDist);
        }

        public void Pause()
        {
            Time.timeScale = 0;
        }

        public void UnPause()
        {
            Time.timeScale = 1;
        }

        public void Init()
        {
            GetAllOf<IWorldScope>()
                    .ForEach(it => it.OnWorldInit());
        }

        public void CleanUp()
        {
            GetAllOf<IWorldScope>()
                    .ForEach(it => it.OnWorldCleanUp());
        }

        private IEnumerable<T> GetAllOf<T>()
        {
            return AppContext.Container.ResolveAll<T>()
                             .Union(GetObjectComponents<T>());
        }

        private List<T> GetObjectComponents<T>()
        {
            return GetObjects().Where(go => go.GetComponent<T>() != null).Select(go => go.GetComponent<T>()).ToList();
        }

        private List<GameObject> GetObjects()
        {
            return GetComponentsInChildren<Transform>(true).Select(it => it.gameObject).ToList();
        }
    }
}