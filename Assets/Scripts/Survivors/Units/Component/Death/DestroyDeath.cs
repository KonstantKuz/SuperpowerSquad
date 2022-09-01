using Survivors.Location.ObjectFactory;
using Survivors.Location.ObjectFactory.Factories;
using UnityEngine;
using Zenject;

namespace Survivors.Units.Component.Death
{
    public class DestroyDeath : MonoBehaviour, IUnitDeath
    {
        [Inject(Id = ObjectFactoryType.Pool)] 
        private IObjectFactory _objectFactory;  
        
        public void PlayDeath()
        {
            _objectFactory.Destroy<Unit>(gameObject);
        }


    }
}
