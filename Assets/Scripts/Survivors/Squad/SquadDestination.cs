using UnityEngine;

namespace Survivors.Squad
{
    public class SquadDestination : MonoBehaviour
    {
        public void SwitchVisibility()
        {
            var meshRenderer = GetComponent<MeshRenderer>();
            meshRenderer.enabled = !meshRenderer.enabled;
        }
    }
}