using System;
using System.Collections.Generic;
using Feofun.Extension;
using Survivors.Extension;
using Survivors.Location;
using Survivors.Location.ObjectFactory;
using Survivors.Location.ObjectFactory.Factories;
using Survivors.Session;
using Survivors.Squad.Formation;
using Survivors.Units.Weapon.Projectiles;
using Survivors.Units.Weapon.Projectiles.Params;
using UnityEngine;
using Zenject;

namespace Survivors.Units.Weapon
{
    public class CircularSawWeapon : MonoBehaviour
    {
        [SerializeField] private CircularSaw _circularSawPrefab;

        private CircularSawsRoot _sawsRoot;
        private readonly List<CircularSaw> _ownedSaws = new List<CircularSaw>();

        [Inject] private World _world;
        
        [Inject(Id = ObjectFactoryType.Instancing)] 
        protected IObjectFactory _objectFactory;

        private CircularSawsRoot SawsRoot => _sawsRoot ??= _world.Squad.GetComponentInChildren<CircularSawsRoot>();
        public IReadOnlyList<CircularSaw> OwnedSaws => _ownedSaws;

        public void Init(UnitType targetType, IProjectileParams projectileParams, Action<GameObject> hitCallback)
        {
            CleanUpSaws();
   
            for (int i = 0; i < projectileParams.Count; i++)
            {
                AddSaw(targetType, projectileParams, hitCallback);
            }

            SawsRoot.OnWeaponInit(this);
        }

        private void AddSaw(UnitType targetType, IProjectileParams projectileParams, Action<GameObject> hitCallback)
        {
            var saw = CreateSaw();
            saw.Init(targetType, projectileParams, hitCallback);
            saw.transform.SetParent(SawsRoot.transform);
            _ownedSaws.Add(saw);
        }
        
        public void OnParamsChanged(IProjectileParams projectileParams)
        {
            _ownedSaws.ForEach(it => it.OnParamsChanged(projectileParams));
            SawsRoot.OnParamsChanged(projectileParams);
        }

        public void CleanUp()
        {
            CleanUpSaws();
            if (_sawsRoot != null) {
                Destroy(_sawsRoot.gameObject);
            }
        }

        private CircularSaw CreateSaw()
        {
            return _objectFactory.Create<CircularSaw>(_circularSawPrefab.gameObject);
        }

        private void CleanUpSaws()
        {
            if (_sawsRoot == null) return;
            _sawsRoot.OnWeaponCleanUp(this);
            _ownedSaws.ForEach(it => Destroy(it.gameObject));
            _ownedSaws.Clear();
        }
    }
}