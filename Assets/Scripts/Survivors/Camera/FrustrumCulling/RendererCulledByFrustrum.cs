using System;
using UniRx.Triggers;
using UnityEngine;
using Zenject;

namespace Survivors.Units.Component.FrustrumCulling
{
    [RequireComponent(typeof(Renderer))]
    public class RendererCulledByFrustrum : MonoBehaviour, ICulledByFrustrum
    {
        private Renderer _renderer;

        [Inject] private FrustrumCullingSystem _cullingSystem;
        private void Awake()
        {
            _renderer = GetComponent<Renderer>();
        }

        private void OnEnable()
        {
            _cullingSystem.Add(this);
        }

        private void OnDisable()
        {
            _cullingSystem.Remove(this);
        }

        public void SetVisible(bool isVisible)
        {
            _renderer.enabled = isVisible;
        }

        public Bounds Bounds => _renderer.bounds;
    }
}