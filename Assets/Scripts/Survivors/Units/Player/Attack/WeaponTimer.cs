using System;
using UniRx;
using UnityEngine;

namespace Survivors.Units.Player.Attack
{
    public class WeaponTimer
    {
        private readonly IReadOnlyReactiveProperty<float> _attackInterval;
        
        private float _lastAttackTime;
        public event Action OnAttackReady;
  
        private bool IsAttackReady => Time.time >= _lastAttackTime + AttackInterval;
        private float AttackInterval => Math.Max(_attackInterval.Value, 0);
        public WeaponTimer(IReadOnlyReactiveProperty<float> attackInterval)
        {
            _attackInterval = attackInterval;
            _lastAttackTime = Time.time - (Time.time % AttackInterval);
        }

        public void ForceDelayNextAttack()
        {
            _lastAttackTime = Time.time;
        }

        public void OnTick()
        {
            if (!IsAttackReady) {
                return;
            }
            OnAttackReady?.Invoke();
            _lastAttackTime = Time.time;
        }
    }
}