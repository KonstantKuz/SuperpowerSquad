using System.Collections.Generic;
using UnityEngine;

namespace Survivors.Utils
{
    public class LineRendererAnchorer : MonoBehaviour
    {
        [SerializeField]
        private List<Transform> _positions;
        [SerializeField]
        private LineRenderer _lineRenderer;

        private void Update()
        {
            for (int i = 0; i < _positions.Count; i++) {
                if (_positions != null && i < _lineRenderer.positionCount) {
                    _lineRenderer.SetPosition(i, _positions[i].transform.position);
                }
            }
        }
    }
}