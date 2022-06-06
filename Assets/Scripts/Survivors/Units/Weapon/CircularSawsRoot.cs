using System.Collections.Generic;
using System.Linq;
using Survivors.Units.Weapon.Projectiles;
using Survivors.Units.Weapon.Projectiles.Params;
using UnityEngine;

namespace Survivors.Units.Weapon
{
    public class CircularSawsRoot : MonoBehaviour
    {
        private Transform _rotationCenter;
        private IProjectileParams _projectileParams;

        private readonly List<CircularSawWeapon> _activeWeapons = new List<CircularSawWeapon>();
        
        private bool Initialized => _rotationCenter != null;

        public void OnWeaponInit(Transform rotationCenter, CircularSawWeapon owner)
        {
            _activeWeapons.Add(owner);
            SetCenter(rotationCenter);
            PlaceSaws();
        }

        private void SetCenter(Transform rotationCenter)
        {
            _rotationCenter = rotationCenter;
        }

        public void OnWeaponCleanUp(CircularSawWeapon owner)
        {
            _activeWeapons.Remove(owner);
        }

        public void OnParamsChanged(IProjectileParams projectileParams)
        {
            _projectileParams = projectileParams;
            PlaceSaws();
        }

        private void PlaceSaws()
        {
            var saws = _activeWeapons.SelectMany(owner => owner.OwnedSaws).ToList();
            var angleStep = 360f / saws.Count;
            var currentPlaceAngle = 0f;
            foreach (var saw in saws)
            {
                saw.SetLocalPlaceByAngle(currentPlaceAngle);                                        
                currentPlaceAngle += angleStep;
            }
        }

        private void Update()
        {
            if(!Initialized) return;
            transform.position = _rotationCenter.position;
            transform.localRotation *= Quaternion.Euler(0, _projectileParams.Speed * Time.deltaTime, 0);
        }
    }
}