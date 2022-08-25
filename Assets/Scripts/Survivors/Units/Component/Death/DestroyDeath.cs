using Survivors.Location.ObjectFactory;
using UnityEngine;
using Zenject;

namespace Survivors.Units.Component.Death
{
    public class DestroyDeath : MonoBehaviour, IUnitDeath
    {
        [Inject]
        private ObjectInstancingFactory objectInstancingFactory;
        
        public void PlayDeath()
        {
            objectInstancingFactory.Destroy<Unit>(gameObject);
        }


    }
}
