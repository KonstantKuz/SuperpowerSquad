using Survivors.Location.ObjectFactory;
using UnityEngine;
using Zenject;

namespace Survivors.Units.Component.Death
{
    public class DestroyDeath : MonoBehaviour, IUnitDeath
    {
        [Inject(Id = ObjectFactoryType.Pool)]
        private IObjectFactory _objectFactory;
        
        public virtual void PlayDeath()
        {
            _objectFactory.Destroy(gameObject);
        }

    
    }
}
