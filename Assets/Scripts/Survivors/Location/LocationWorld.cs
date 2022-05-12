using UnityEngine;

namespace Survivors.Location
{
    public class LocationWorld : MonoBehaviour
    {
        [SerializeField] private GameObject _spawnContainer;
        public GameObject SpawnContainer => _spawnContainer;
    }
}
