﻿using System;
using Logger.Assets.Scripts;
using Survivors.Units.Component.Health;
using Survivors.Units.Target;
using Survivors.Units.Weapon.Projectiles.Params;
using UnityEngine;
using ILogger = Logger.Assets.Scripts.ILogger;

namespace Survivors.Units.Weapon
{
    public class MeleeWeapon : BaseWeapon
    {
        private static readonly ILogger _logger = LoggerFactory.GetLogger<MeleeWeapon>();
        
        public override void Fire(ITarget target, IProjectileParams chargeParams, Action<GameObject> hitCallback)
        {
            var targetObj = target as MonoBehaviour;
            if (targetObj == null)
            {
                _logger.Warn("Target is not a monobehaviour");
                return;
            }

            if (targetObj.GetComponent<IDamageable>() == null)
            {
                _logger.Warn("Target has no damageable component");
                return;
            }
            hitCallback?.Invoke(targetObj.gameObject);
        }
    }
}