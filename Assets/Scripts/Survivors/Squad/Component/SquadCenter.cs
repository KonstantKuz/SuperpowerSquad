using UnityEngine;

namespace Survivors.Squad.Component
{
    public class SquadCenter : MonoBehaviour
    {
        public void SwitchVisibility()
        {
            var meshRenderer = GetComponent<MeshRenderer>();
            meshRenderer.enabled = !meshRenderer.enabled;
        }
    }
}