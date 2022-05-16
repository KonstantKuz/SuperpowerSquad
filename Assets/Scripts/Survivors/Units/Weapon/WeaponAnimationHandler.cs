using System;
using JetBrains.Annotations;
using UnityEngine;

namespace Survivors.Units.Weapon
{
    public class WeaponAnimationHandler : MonoBehaviour
    {
        public Action OnFireEvent;

        [UsedImplicitly]
        public void Fire()
        {
            OnFireEvent?.Invoke();
        }
    }
}