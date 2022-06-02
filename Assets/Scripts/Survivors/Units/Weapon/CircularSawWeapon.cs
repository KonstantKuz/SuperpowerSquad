using System;
using Feofun.Extension;
using Survivors.Extension;
using Survivors.Location;
using Survivors.Location.Service;
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

        [Inject] private World _world;
        [Inject] private WorldObjectFactory _worldObjectFactory;

        public int SawsCount { get; set; }
        
        public void Init(Transform rotationCenter, IProjectileParams projectileParams)
        {
            _sawsRoot = _world.Spawn.GetComponentInChildren<CircularSawsRoot>();
            if (_sawsRoot != null) return;

            _sawsRoot = new GameObject("SawsRoot").AddComponent<CircularSawsRoot>();
            _sawsRoot.transform.SetParent(_world.Spawn.transform);
            _sawsRoot.Init(rotationCenter, projectileParams.Speed);
        }

        public void AddSaw(UnitType targetType, IProjectileParams projectileParams, Action<GameObject> hitCallback)
        {
            var saw = CreateSaw();
            saw.Init(targetType, projectileParams, hitCallback);
            _sawsRoot.AddSaw(saw, this);
        }
        
        public void OnParamsChanged(IProjectileParams projectileParams)
        {
            _sawsRoot.OnParamsChanged(projectileParams);
        }

        public void CleanUpSaws()
        {
            if (_sawsRoot == null) return;
            _sawsRoot.CleanUpSaws(this);
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
            return _worldObjectFactory.CreateObject(_circularSawPrefab.gameObject).RequireComponent<CircularSaw>();
        }
    }
}