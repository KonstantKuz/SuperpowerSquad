using Survivors.Units.Component.FrustrumCulling;
using UnityEngine;
using Zenject;

namespace Survivors.Units.Component
{
    [RequireComponent(typeof(CachedSkinRenderer))]
    public class CachedSkinRendererCulling : MonoBehaviour, ICulledByFrustrum
    {
        private CachedSkinRenderer _cachedSkinRenderer;
        
        [Inject] private FrustrumCullingSystem _cullingSystem;

        private void Awake()
        {
            _cachedSkinRenderer = GetComponent<CachedSkinRenderer>();
        }

        public void SetVisible(bool isVisible)
        {
            _cachedSkinRenderer.enabled = isVisible;
        }

        public Bounds Bounds => _cachedSkinRenderer.Bounds;

        private void OnEnable()
        {
            _cullingSystem.Add(this);
        }

        private void OnDisable()
        {
            _cullingSystem.Remove(this);
        }
    }
}