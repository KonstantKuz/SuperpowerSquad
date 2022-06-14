using System.Collections.Generic;
using Survivors.Location;
using UnityEngine;
using Zenject;

namespace Survivors.Camera
{
    [RequireComponent(typeof(UnityEngine.Camera))]
    public class CameraFrustumDrawer : MonoBehaviour
    {
        private UnityEngine.Camera _camera;
        [Inject] private World _world;

        private void Awake()
        {
            _camera = GetComponent<UnityEngine.Camera>();
        }

        private void OnDrawGizmos()
        {
            if (_camera == null || _world == null)
            {
                return;
            }

            Gizmos.color = Color.red;

            var rays = new List<Ray>
            {
                _camera.ViewportPointToRay(new Vector2(0, 0)),
                _camera.ViewportPointToRay(new Vector2(1, 0)),
                _camera.ViewportPointToRay(new Vector2(1, 1)),
                _camera.ViewportPointToRay(new Vector2(0, 1)),
            };
            
            for (int i = 0; i < rays.Count; i++)
            {
                var groundIntersection = _world.GetGroundIntersection(rays[i]);
                Gizmos.DrawLine(transform.position, groundIntersection);
                
                var nextRayIndex = (i + 1) % rays.Count;
                var nextRayGroundIntersection = _world.GetGroundIntersection(rays[nextRayIndex]);
                Gizmos.DrawLine(nextRayGroundIntersection, groundIntersection);
            }
        }
    }
}