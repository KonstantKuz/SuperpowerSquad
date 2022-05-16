using UnityEngine;

namespace Survivors.GameWorld
{
    public class World : GameWorld
    {
        [SerializeField] private GameObject _spawn;

        public GameObject Spawn => _spawn;
        
    }
}