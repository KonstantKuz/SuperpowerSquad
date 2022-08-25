using Survivors.Location.ObjectFactory.Factories;
using UnityEngine;
using Zenject;

namespace Survivors.Units.Component.Death
{
    public class DestroyDeath : MonoBehaviour, IUnitDeath
    {
        [Inject(Id = ObjectFactoryType.Pool)] 
        protected IObjectFactory _objectFactory;  
        
        public void PlayDeath()
        {
            _objectFactory.Destroy<Unit>(gameObject);
        }


    }
}
