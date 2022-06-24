using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Survivors.Extension;
using Survivors.Location.Service;
using Survivors.Units.Component.TargetSearcher;
using Survivors.Units.Target;
using Survivors.Units.Weapon.Projectiles;
using Survivors.Units.Weapon.Projectiles.Params;
using UnityEngine;
using Zenject;

namespace Survivors.Units.Weapon
{
    public class LightningStrikeWeapon : BaseWeapon
    {
        [SerializeField] private LightningStrike _lightningStrike;

        private LargestHealthEnemySearcher _largestHealthEnemySearcher;
        
        [Inject] private WorldObjectFactory _worldObjectFactory;
        
        private void Awake()
        {
            _largestHealthEnemySearcher = gameObject.RequireComponentInParent<LargestHealthEnemySearcher>();
        }

        public override void Fire(ITarget target, IProjectileParams projectileParams, Action<GameObject> hitCallback)
        {
            var targets = BuildTargets(target, projectileParams.Count);
            foreach (var finalTarget in targets)
            {
                FireSingleStrike(finalTarget, projectileParams, hitCallback);
            }
        }

        private IEnumerable<ITarget> BuildTargets(ITarget initialTarget, int count)
        {
            var targets = new List<ITarget> { initialTarget };
            var additionalTargetsCount = count - 1;
            for (int i = 0; i < additionalTargetsCount; i++)
            {
                var otherTarget = _largestHealthEnemySearcher.FindExcept(targets);
                if (otherTarget == null)
                {
                    targets.Add(initialTarget);
                    continue;
                }
                targets.Add(otherTarget);
            }
            return targets;
        }

        private void FireSingleStrike(ITarget target, IProjectileParams projectileParams, Action<GameObject> hitCallback)
        {
            var lightning = CreateLightning();
            lightning.Launch(target, projectileParams, hitCallback);
        }

        private Projectile CreateLightning()
        {
            return _worldObjectFactory.CreateObject(_lightningStrike.gameObject).GetComponent<Projectile>();
        }
    }
}
