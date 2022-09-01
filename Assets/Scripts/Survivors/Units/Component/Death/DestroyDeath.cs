using Feofun.Components;
using Survivors.Location.ObjectFactory.Factories;
using UnityEngine;
using Zenject;

namespace Survivors.Units.Component.Death
{
    public class DestroyDeath : MonoBehaviour, IUnitDeath, IInitializable<IUnit>
    {
        [Inject]
        private ObjectPoolFactory _objectFactory;

        private Unit _owner;
        
        public void Init(IUnit owner)
        {
            _owner = (Unit) owner;
        }
        
        public void PlayDeath()
        {
            _objectFactory.Destroy(_owner.ObjectId, gameObject);
        }

    
    }
}
