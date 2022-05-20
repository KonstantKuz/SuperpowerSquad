using System;
using Survivors.Location.Service;
using Survivors.Units.Target;
using Survivors.Units.Weapon.Projectiles;
using UnityEngine;
using Zenject;

namespace Survivors.Units.Weapon
{
    public class BeamWeapon : BaseWeapon
    {
        [SerializeField]
        private Transform _barrel;
        [SerializeField]
        private Beam _beam;
        [Inject]
        private WorldObjectFactory _objectFactory;

        private Vector3 _barrelPos;

        public override void Fire(ITarget target, ProjectileParams projectileParams, Action<GameObject> hitCallback)
        {
            var beam = CreateBeam();
            var rotationToTarget = RangedWeapon.GetShootRotation(_barrelPos, target.Center.position);
            beam.transform.SetPositionAndRotation(_barrelPos, rotationToTarget);
            beam.Launch(target, projectileParams, hitCallback, _barrel);
        }

        private Beam CreateBeam()
        {
            return _objectFactory.CreateObject(_beam.gameObject).GetComponent<Beam>();
        }

        private void LateUpdate()
        {
            _barrelPos = _barrel.position;
        }
    }
}