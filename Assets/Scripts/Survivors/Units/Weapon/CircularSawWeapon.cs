using System;
using System.Collections.Generic;
using Survivors.Location;
using Survivors.Location.Service;
using Survivors.Units.Weapon.Projectiles;
using UnityEngine;
using Zenject;

namespace Survivors.Units.Weapon
{
    public class CircularSawWeapon : MonoBehaviour
    {
        [SerializeField] private CircularSaw _circularSawPrefab;

        private Transform _rotateAround;
        private ProjectileParams _projectileParams;
        private Transform _sawsRoot;
        private List<CircularSaw> _saws;

        [Inject] private World _world;
        [Inject] private WorldObjectFactory _worldObjectFactory;

        public void Init(Transform rotateAround, ProjectileParams projectileParams)
        {
            _rotateAround = rotateAround;
            _projectileParams = projectileParams;
            _sawsRoot = new GameObject("SawsRoot").transform;
            _sawsRoot.SetParent(_world.SpawnContainer.transform);
            
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

        public void CleanUp()
        {
            _saws.ForEach(Destroy);
            _saws.Clear();
        }

        private void Update()
        {
            _sawsRoot.localRotation *= Quaternion.Euler(0, _projectileParams.Speed * Time.deltaTime, 0);
            _sawsRoot.position = _rotateAround.position;
        }

        private void PlaceSaws()
        {
            float angleStep = 360f / _saws.Count;
            float currentPlaceAngle = 0;
            for (int i = 0; i < _saws.Count; i++)
            {
                _saws[i].SetLocalPlaceByAngle(currentPlaceAngle);                                        
                currentPlaceAngle += angleStep;
            }
        }

        private CircularSaw CreateSaw()
        {
            return _worldObjectFactory.CreateObject(_circularSawPrefab.gameObject).GetComponent<CircularSaw>();
        }
    }
}