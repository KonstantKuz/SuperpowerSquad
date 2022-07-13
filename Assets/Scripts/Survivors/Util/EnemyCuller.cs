using SuperMaxim.Core.Extensions;
using Survivors.Extension;
using UnityEngine;

namespace Survivors.Util
{
    [RequireComponent(typeof(Collider))]
    public class EnemyCuller : MonoBehaviour
    {
        private Animator _animator;
        private Collider _collider;
        private Renderer[] _renderers;
        
        private bool? _componentsEnabled;
        private Bounds Bounds => _collider.bounds;
        
        private bool ComponentsEnabled
        {
            set
            {
                if (value == _componentsEnabled && _componentsEnabled.HasValue) {
                    return;
                }
                _componentsEnabled = value;
                _animator.enabled = _componentsEnabled.Value;
                _renderers.ForEach(it => it.enabled = _componentsEnabled.Value);
            }
        }
        private void Awake()
        {
            _animator = gameObject.RequireComponentInChildren<Animator>();
            _collider = GetComponent<Collider>();
            _renderers = gameObject.GetComponentsInChildren<Renderer>();
        }
        private void Update()
        {
            var frustumPlanes = GeometryUtility.CalculateFrustumPlanes(UnityEngine.Camera.main);
            if (IsVisible(Bounds, frustumPlanes)) {
                ComponentsEnabled = true;
            } else {
                ComponentsEnabled = false;
            }
        }
        private bool IsVisible(Bounds bounds, Plane[] frustrumPlanes)
        {
            return GeometryUtility.TestPlanesAABB(frustrumPlanes,bounds);
        }
        
    }
}