using Survivors.Units.Component.FrustrumCulling;
using UnityEngine;
using Zenject;

namespace Survivors.Camera.FrustrumCulling
{
    [RequireComponent(typeof(Collider))]
    public class TornadoCulledByFrustrum : MonoBehaviour, ICulledByFrustrum
    {
        [SerializeField] private GameObject _rootVfx;

        private Collider _collider;
        
        [Inject] private FrustrumCullingSystem _cullingSystem;

        public void SetVisible(bool isVisible)
        {
            _rootVfx.SetActive(isVisible);
        }

        public Bounds Bounds => _collider.bounds;

        private void Awake()
        {
            _collider = GetComponent<Collider>();
        }

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