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
        private Beam _beam;
        [Inject]
        private WorldObjectFactory _objectFactory;

        public override void Fire(ITarget target, ProjectileParams projectileParams, Action<GameObject> hitCallback)
        {
            var beam = CreateBeam();
            var rotationToTarget = RangedWeapon.GetShootRotation(BarrelPos, target.Center.position);
            beam.transform.SetPositionAndRotation(BarrelPos, rotationToTarget);
            beam.Launch(target, projectileParams, hitCallback, Barrel);
        }

        private Beam CreateBeam()
        {
            return _objectFactory.CreateObject(_beam.gameObject).GetComponent<Beam>();
        }
    }
}