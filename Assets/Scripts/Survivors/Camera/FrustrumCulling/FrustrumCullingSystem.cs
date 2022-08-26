using System.Collections.Generic;
using UnityEngine;

namespace Survivors.Units.Component.FrustrumCulling
{
    [RequireComponent(typeof(UnityEngine.Camera))]
    public class FrustrumCullingSystem: MonoBehaviour
    {
        private readonly HashSet<ICulledByFrustrum> _items = new HashSet<ICulledByFrustrum>();
        private UnityEngine.Camera _camera;

        public void Add(ICulledByFrustrum item)
        {
            _items.Add(item);
        }

        public void Remove(ICulledByFrustrum item)
        {
            _items.Remove(item);
        }

        private void Awake()
        {
            _camera = GetComponent<UnityEngine.Camera>();
        }

        private void Update()
        {
            var planes = GeometryUtility.CalculateFrustumPlanes(_camera);
            foreach (var item in _items)
            {
                item.SetVisible(GeometryUtility.TestPlanesAABB(planes, item.Bounds));
            }
        }
    }
}