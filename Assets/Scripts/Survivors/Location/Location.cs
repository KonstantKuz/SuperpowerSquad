using UnityEngine;

namespace Survivors.Location
{
    public class Location : GameWorld
    {
        [SerializeField] private GameObject _spawnContainer;

        public GameObject SpawnContainer => _spawnContainer;
        
    }
}