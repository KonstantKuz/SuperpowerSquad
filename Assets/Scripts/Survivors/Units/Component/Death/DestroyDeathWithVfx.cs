using Feofun.Vfx;
using Survivors.Location.ObjectFactory.Factories;
using UnityEngine;
using Zenject;

namespace Survivors.Units.Component.Death
{
    public class DestroyDeathWithVfx : DestroyDeath
    {
        [SerializeField]
        private GameObject _vfxPrefab;
        
        [Inject]
        private ObjectInstancingFactory _objectInstancingFactory;
        
        public override void PlayDeath()
        {
            var vfx = _objectInstancingFactory.Create<AutoDestroyVfx>(_vfxPrefab);
            vfx.transform.position = transform.position;
            base.PlayDeath();
        }

    }
}