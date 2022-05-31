using System;
using System.Collections.Generic;
using Survivors.Extension;
using Survivors.Location;
using Survivors.Location.Service;
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

        private Transform _rotationCenter;
        private IProjectileParams _projectileParams;
        private Transform _sawsRoot;
        private List<CircularSaw> _saws;

        [Inject] private World _world;
        [Inject] private WorldObjectFactory _worldObjectFactory;

        private bool Initialized => _sawsRoot != null;

        public void Init(Transform rotationCenter, IProjectileParams projectileParams)
        {
            _rotationCenter = rotationCenter;
            _projectileParams = projectileParams;
            _sawsRoot = new GameObject("SawsRoot").transform;
            _sawsRoot.SetParent(_world.Spawn.transform);
            _saws = new List<CircularSaw>();
        }
        
        public void AddSaw(UnitType targetType, Action<GameObject> hitCallback)
        {
            var saw = CreateSaw();
            saw.Init(targetType, _projectileParams, hitCallback);
            saw.transform.SetParent(_sawsRoot);
            _saws.Add(saw);
            PlaceSaws();
        }
        
        public void UpdateParams(IProjectileParams projectileParams)
        {
            _projectileParams = projectileParams;
            foreach (var saw in _saws)
            {
                saw.UpdateParams(_projectileParams);
            }
            PlaceSaws();
        }

        public void CleanUpSaws()
        {
            _saws?.ForEach(Destroy);
            _saws?.Clear();
        } 
        public void CleanUp()
        {
            CleanUpSaws();
            if (_sawsRoot != null) {
                Destroy(_sawsRoot.gameObject);
            }
        }

        private void Update()
        {
            if (!Initialized) {
                return;
            }
            _sawsRoot.localRotation *= Quaternion.Euler(0, _projectileParams.Speed * Time.deltaTime, 0);
            _sawsRoot.position = _rotationCenter.position;
        }

        private void PlaceSaws()
        {
            var angleStep = 360f / _saws.Count;
            var currentPlaceAngle = 0f;
            for (int i = 0; i < _saws.Count; i++)
            {
                _saws[i].SetLocalPlaceByAngle(currentPlaceAngle);                                        
                currentPlaceAngle += angleStep;
            }
        }

        private CircularSaw CreateSaw()
        {
            return _worldObjectFactory.CreateObject(_circularSawPrefab.gameObject).RequireComponent<CircularSaw>();
        }
    }
}