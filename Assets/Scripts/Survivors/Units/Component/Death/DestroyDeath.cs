using Feofun.Components;
using Survivors.Location.Service;
using UnityEngine;
using Zenject;

namespace Survivors.Units.Component.Death
{
    public class DestroyDeath : MonoBehaviour, IUnitDeath, IInitializable<IUnit>
    {
        [Inject]
        private WorldObjectFactory _world;

        private IUnit _owner;
        
        public void Init(IUnit owner)
        {
            _owner = owner;
        }
        
        public void PlayDeath()
        {
            //Destroy(gameObject);
            _world.ReleaseObject((Unit)_owner);
        }


    }
}
