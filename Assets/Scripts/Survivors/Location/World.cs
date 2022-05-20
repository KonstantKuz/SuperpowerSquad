using Feofun.App;
using Survivors.Session;
using UnityEngine;

namespace Survivors.Location
{
    public class World : MonoBehaviour
    {
        [SerializeField] private Transform _ground;
        [SerializeField] private GameObject _spawnContainer;   
        [SerializeField] private Squad.Squad _squad;

        public Transform Ground => _ground;
        public GameObject SpawnContainer => _spawnContainer;  
        public Squad.Squad Squad => _squad;

        public Vector3 GetGroundIntersection(Ray withRay)
        {
            var plane = new Plane(Ground.up, Ground.position);
            plane.Raycast(withRay, out var intersectionDist);
            return withRay.GetPoint(intersectionDist);
        }
        public void CleanUp()
        {
            var services = AppContext.Container.ResolveAll<IWorldCleanUp>();
            services.ForEach(it => it.OnWorldCleanUp());
            
        }
    }
}
