using System.Collections.Generic;
using System.Linq;
using SuperMaxim.Core.Extensions;
using Survivors.Units.Weapon.Projectiles;
using Survivors.Units.Weapon.Projectiles.Params;
using UnityEngine;

namespace Survivors.Units.Weapon
{
    public class CircularSawsRoot : MonoBehaviour
    {
        private Transform _rotationCenter;
        private float _rotationSpeed;

        private List<CircularSaw> _saws;
        
        private bool Initialized => _rotationCenter != null;
        public List<CircularSaw> Saws => _saws ??= new List<CircularSaw>();

        public void Init(Transform rotationCenter, float rotationSpeed)
        {
            _rotationCenter = rotationCenter;
            _rotationSpeed = rotationSpeed;
        }

        public void AddSaw(CircularSaw newSaw, CircularSawWeapon fromWeapon)
        {
            fromWeapon.SawsCount++;
            newSaw.transform.SetParent(transform);
            Saws.Add(newSaw);
            PlaceSaws();
        }

        public void OnParamsChanged(IProjectileParams projectileParams)
        {
            Saws.ForEach(it => it.OnParamsChanged(projectileParams));
            PlaceSaws();
        }

        public void CleanUpSaws(CircularSawWeapon fromWeapon)
        {
            _saws?.Take(fromWeapon.SawsCount).ForEach(Destroy);
            fromWeapon.SawsCount = 0;
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

        private void Update()
        {
            if(!Initialized) return;
            transform.position = _rotationCenter.position;
            transform.localRotation *= Quaternion.Euler(0, _rotationSpeed * Time.deltaTime, 0);
        }
    }
}