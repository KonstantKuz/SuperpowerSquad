using System;
using JetBrains.Annotations;
using UnityEngine;

namespace Survivors.Units.Weapon
{
    public class WeaponAnimationHandler : MonoBehaviour
    {
        public Action OnFireEvent;      
        public Action OnFireCompleted;
        
        [UsedImplicitly]
        public void Fire()
        {
            OnFireEvent?.Invoke();
        } 
        [UsedImplicitly]
        public void FireCompleted()
        {
            OnFireCompleted?.Invoke();
        }
    }
}