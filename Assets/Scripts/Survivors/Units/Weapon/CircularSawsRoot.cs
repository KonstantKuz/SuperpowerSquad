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

        private Dictionary<CircularSawWeapon, List<CircularSaw>> _sawsByOwner;
        
        private bool Initialized => _rotationCenter != null;
        private Dictionary<CircularSawWeapon, List<CircularSaw>> SawsByOwner => _sawsByOwner ??= new Dictionary<CircularSawWeapon, List<CircularSaw>>();

        public void SetCenter(Transform rotationCenter)
        {
            _rotationCenter = rotationCenter;
        }

        public void OnWeaponInit(CircularSawWeapon owner)
        {
            SawsByOwner[owner] = owner.OwnSaws;
            PlaceSaws();
        }

        public void OnWeaponCleanUp(CircularSawWeapon owner)
        {
            SawsByOwner.Remove(owner);
        }

        public void OnParamsChanged(IProjectileParams projectileParams)
        {
            _projectileParams = projectileParams;
            PlaceSaws();
        }

        private void PlaceSaws()
        {
            var saws = _sawsByOwner.Values.SelectMany(owner => owner).ToList();
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