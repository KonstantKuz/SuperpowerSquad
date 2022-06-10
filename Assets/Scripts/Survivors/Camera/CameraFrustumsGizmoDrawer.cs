using Survivors.Location;
using UnityEngine;
using Zenject;

namespace Survivors.Camera
{
    [RequireComponent(typeof(UnityEngine.Camera))]
    public class CameraFrustumsGizmoDrawer : MonoBehaviour
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

            var bottomLeft = _camera.ViewportPointToRay(new Vector2(0, 0));
            var bottomRight = _camera.ViewportPointToRay(new Vector2(1, 0));
            var topLeft = _camera.ViewportPointToRay(new Vector2(0, 1));
            var topRight = _camera.ViewportPointToRay(new Vector2(1, 1));
        
            Gizmos.DrawLine(transform.position, _world.GetGroundIntersection(bottomLeft));
            Gizmos.DrawLine(transform.position, _world.GetGroundIntersection(bottomRight));
            Gizmos.DrawLine(transform.position, _world.GetGroundIntersection(topLeft));
            Gizmos.DrawLine(transform.position, _world.GetGroundIntersection(topRight));
            Gizmos.DrawLine(_world.GetGroundIntersection(bottomLeft), _world.GetGroundIntersection(bottomRight));
            Gizmos.DrawLine(_world.GetGroundIntersection(topLeft), _world.GetGroundIntersection(topRight));
            Gizmos.DrawLine(_world.GetGroundIntersection(bottomLeft), _world.GetGroundIntersection(topLeft));
            Gizmos.DrawLine(_world.GetGroundIntersection(bottomRight), _world.GetGroundIntersection(topRight));
        }
    }
}