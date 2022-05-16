using UnityEngine;

namespace Survivors.Location
{
    public class World : MonoBehaviour
    {
        [SerializeField] private Transform _ground;
        [SerializeField] private GameObject _spawnContainer;

        public Transform Ground => _ground;
        public GameObject SpawnContainer => _spawnContainer;

        public Vector3 GetGroundIntersection(Ray withRay)
        {
            var plane = new Plane(Ground.up, Ground.position);
            plane.Raycast(withRay, out var intersectionDist);
            return withRay.GetPoint(intersectionDist);
        }
    }
}
