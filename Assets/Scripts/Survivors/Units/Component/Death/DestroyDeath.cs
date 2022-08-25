using Survivors.Location.ObjectFactory;
using UnityEngine;
using Zenject;

namespace Survivors.Units.Component.Death
{
    public class DestroyDeath : MonoBehaviour, IUnitDeath
    {
        [Inject(Id = ObjectFactoryType.Instancing)] 
        protected IObjectFactory _objectFactory;  
        
        public void PlayDeath()
        {
            _objectFactory.Destroy<Unit>(gameObject);
        }


    }
}
