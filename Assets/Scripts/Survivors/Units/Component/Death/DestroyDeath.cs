using Survivors.Location.ObjectFactory;
using UnityEngine;
using Zenject;

namespace Survivors.Units.Component.Death
{
    public class DestroyDeath : MonoBehaviour, IUnitDeath
    {
        [Inject(Id = ObjectFactoryType.Pool)]
        private IObjectFactory _objectFactory;

        private Unit _owner;
        
        public void PlayDeath()
        {
            _objectFactory.Destroy(gameObject);
        }

    
    }
}
