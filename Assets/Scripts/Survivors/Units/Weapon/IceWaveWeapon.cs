using System;
using Feofun.Modifiers.Modifiers;
using Survivors.Extension;
using Survivors.Location.ObjectFactory;
using Survivors.Modifiers;
using Survivors.Units.Weapon.Projectiles;
using Survivors.Units.Weapon.Projectiles.Params;
using UnityEngine;
using Zenject;

namespace Survivors.Units.Weapon
{
    public class IceWaveWeapon : MonoBehaviour
    {
        [SerializeField] private IceWave _iceWave;
        
        [Inject(Id = ObjectFactoryType.Instancing)] 
        private IObjectFactory _objectFactory;
        
        public void Fire(Transform parent, UnitType targetType, IProjectileParams projectileParams,
            Action<GameObject> hitCallback)
        {
            var wave = CreateWave();
            wave.Launch(targetType, parent, projectileParams, hitCallback);
        }

        private IceWave CreateWave()
        {
            return _objectFactory.Create<IceWave>(_iceWave.gameObject);
        }
    }
}
