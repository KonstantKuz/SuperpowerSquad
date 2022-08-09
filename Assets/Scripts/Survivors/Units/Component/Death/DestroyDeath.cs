using Feofun.Components;
using Survivors.Location.Service;
using UnityEngine;
using Zenject;

namespace Survivors.Units.Component.Death
{
    public class DestroyDeath : MonoBehaviour, IUnitDeath
    {
        [Inject]
        private WorldObjectFactory _worldObjectFactory;
        
        public void PlayDeath()
        {
            _worldObjectFactory.DestroyObject<Unit>(gameObject);
        }


    }
}
