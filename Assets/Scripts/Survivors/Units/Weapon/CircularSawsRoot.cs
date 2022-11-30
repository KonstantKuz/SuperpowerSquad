using System.Collections.Generic;
using System.Linq;
using Feofun.Components;
using Survivors.Units.Weapon.Projectiles.Params;
using UnityEngine;

namespace Survivors.Units.Weapon
{
    public class CircularSawsRoot : MonoBehaviour, IInitializable<Squad.Squad>
    {
        private Transform _rotationCenter;
        private IProjectileParams _projectileParams;
        private readonly List<CircularSawWeapon> _activeWeapons = new List<CircularSawWeapon>();
        private bool Initialized => _projectileParams != null && _rotationCenter != null;

        public void Init(Squad.Squad squad)
        {
            _rotationCenter = squad.Center.transform;
        }

        public void OnWeaponInit(CircularSawWeapon owner)
        {
            _activeWeapons.Add(owner);
            PlaceSaws();
        }
        
        public void OnWeaponCleanUp(CircularSawWeapon owner)
        {
            _activeWeapons.Remove(owner);
            PlaceSaws();
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