using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Survivors.Enemy.Spawn
{
    public class SpawnerDebugger : MonoBehaviour
    {
        private const float COLOR_ALPHA = 0.3f;
        private const float LIFE_TIME = 2f;
        private List<DebugInfo> _infos = new List<DebugInfo>();

        public void Debug(Vector3 place, float waveRadius, bool status)
        {
            _infos.Add(new DebugInfo() {
                Place = place, 
                CreationTime = Time.time,
                WaveRadius = waveRadius,
                Status = status,
            });
        }

        private void OnDrawGizmos()
        {
            var infos = _infos.ToList();
            foreach (var info in infos)
            {
                if (info.CreationTime + LIFE_TIME >= Time.time)
                {
                    _infos.Remove(info);
                    continue;
                }
                
                DrawDebugSphere(info);
            }
        }

        private static void DrawDebugSphere(DebugInfo info)
        {
            var color = info.Status ? Color.red : Color.green;
            color.a = COLOR_ALPHA;
            Gizmos.color = color;
            Gizmos.DrawSphere(info.Place, info.WaveRadius);
        }

        private class DebugInfo
        {
            public Vector3 Place; 
            public float CreationTime;           
            public float WaveRadius;  
            public bool Status;
        }
    }
}