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
        [Inject] private ObjectInstancingFactory objectInstancingFactory;
        
        public void Fire(Transform parent, UnitType targetType, IProjectileParams projectileParams,
            Action<GameObject> hitCallback)
        {
            var wave = CreateWave();
            wave.Launch(targetType, parent, projectileParams, hitCallback);
        }

        private IceWave CreateWave()
        {
            return objectInstancingFactory.CreateObject(_iceWave.gameObject).RequireComponent<IceWave>();
        }
    }
}
