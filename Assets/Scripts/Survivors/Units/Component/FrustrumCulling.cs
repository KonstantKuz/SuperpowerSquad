using UnityEngine;

namespace Survivors.Units.Component
{
    [RequireComponent(typeof(Renderer))]
    public class FrustrumCulling : MonoBehaviour
    {
        private Renderer _renderer;
        private void Awake()
        {
            _renderer = GetComponent<Renderer>();
        }

        private void Update()
        {
            var planes = GeometryUtility.CalculateFrustumPlanes(UnityEngine.Camera.main);
            _renderer.enabled = GeometryUtility.TestPlanesAABB(planes, _renderer.bounds);
        }
    }
}