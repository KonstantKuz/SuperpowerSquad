using System;
using Survivors.Location.ObjectFactory;
using Survivors.Location.ObjectFactory.Factories;
using Survivors.Units.Target;
using Survivors.Units.Weapon.Projectiles;
using Survivors.Units.Weapon.Projectiles.Params;
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
        
        [Inject(Id = ObjectFactoryType.Instancing)] 
        private IObjectFactory _objectFactory;

        public Vector3 BarrelPos { get; private set; }

        public override void Fire(ITarget target, IProjectileParams projectileParams, Action<GameObject> hitCallback)
        {
            var beam = CreateBeam();
            var rotationToTarget = RangedWeapon.GetShootRotation(BarrelPos, target.Center.position, true);
            beam.transform.SetPositionAndRotation(BarrelPos, rotationToTarget);
            beam.Launch(target, projectileParams, hitCallback, _barrel);
        }

        private Beam CreateBeam()
        {
            return _objectFactory.Create<Beam>(_beam.gameObject, _barrel);
        }

        private void LateUpdate()
        {
            BarrelPos = _barrel.position;
        }
    }
}