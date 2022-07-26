using Survivors.Location.Service;
using UnityEngine;
using Zenject;

namespace Survivors.Units.Component.Death
{
    public class DestroyDeath : MonoBehaviour, IUnitDeath
    {
        [Inject]
        private WorldObjectFactory _world;
        public void PlayDeath()
        {
            //Destroy(gameObject);
            _world.ReleaseObject(gameObject);
        }
    }
}
