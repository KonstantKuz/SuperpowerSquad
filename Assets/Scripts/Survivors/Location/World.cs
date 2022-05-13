using UnityEngine;

namespace Survivors.Location
{
    public class World : MonoBehaviour
    {
        [SerializeField] private Transform _player;
        [SerializeField] private GameObject _spawnContainer;

        public Transform Player => _player;
        public GameObject SpawnContainer => _spawnContainer;
    }
}
