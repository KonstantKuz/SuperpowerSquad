using UnityEngine;

namespace Survivors.Camera.FrustrumCulling
{
    public interface ICulledByFrustrum
    {
        public void SetVisible(bool isVisible);
        public Bounds Bounds { get; }
    }
}