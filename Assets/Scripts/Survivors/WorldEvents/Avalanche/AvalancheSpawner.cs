using System.Collections;
using UnityEngine;

namespace Survivors.WorldEvents.Avalanche
{
    public class AvalancheSpawner : MonoBehaviour
    {
        [SerializeField] private int _spawnCount;

        
        private IEnumerator Start()
        {
            yield return new WaitForSeconds(5);
            SpawnCobblestones();
        }

        private void SpawnCobblestones()
        {
            for (int i = 0; i < _spawnCount; i++)
            {
                
            }
        }
    }
}
