using System.Collections.Generic;
using System.Linq;
using EasyButtons;
using Feofun.App;
using SuperMaxim.Core.Extensions;
using Survivors.Location.Service;
using Survivors.Session;
using UnityEngine;
using Zenject;

namespace Survivors.Location
{
    public class World : MonoBehaviour
    {
        [SerializeField] private Transform _ground;
        [SerializeField] private GameObject _spawnContainer;   
        [SerializeField] private Squad.Squad _squad;

        [Inject] private WorldObjectFactory _worldObjectFactory;

        public Transform Ground => _ground;
        public GameObject SpawnContainer => _spawnContainer;  
        public Squad.Squad Squad => _squad;

        public Vector3 GetGroundIntersection(Ray withRay)
        {
            var plane = new Plane(Ground.up, Ground.position);
            plane.Raycast(withRay, out var intersectionDist);
            return withRay.GetPoint(intersectionDist);
        }

        [Button]
        public void CleanUp()
        {
            var services = AppContext.Container.ResolveAll<IWorldCleanUp>();
            services.ForEach(it => it.OnWorldCleanUp());
            var gameObjects = GetObjectComponents<IWorldCleanUp>().Except(services);
            gameObjects.ForEach(it => it.OnWorldCleanUp());
            
            _worldObjectFactory.DestroyAllObjects();
        }

        public List<T> GetObjectComponents<T>()
        {
            return GetObjects()
                .Where(go => go.GetComponent<T>() != null)
                .Select(go => go.GetComponent<T>())
                .ToList();
        }

        public List<GameObject> GetObjects()
        {
            return GetComponentsInChildren<Transform>(true)
                .Select(it => it.gameObject)
                .ToList();
        }
    }
}
