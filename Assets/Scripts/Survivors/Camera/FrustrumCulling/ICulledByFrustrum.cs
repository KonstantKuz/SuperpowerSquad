using UnityEngine;

namespace Survivors.Units.Component.FrustrumCulling
{
    public interface ICulledByFrustrum
    {
        public void SetVisible(bool isVisible);
        public Bounds Bounds { get; }
    }
}