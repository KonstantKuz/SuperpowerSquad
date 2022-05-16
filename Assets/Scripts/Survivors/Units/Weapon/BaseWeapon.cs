using System;
using Survivors.Units.Target;
using UnityEngine;

namespace Survivors.Units.Weapon
{
    public abstract class BaseWeapon : MonoBehaviour
    {
        public abstract void Fire(ITarget target, ChargeParams chargeParams, Action<GameObject> hitCallback);
    }
}