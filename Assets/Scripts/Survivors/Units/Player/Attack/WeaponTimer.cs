using System;
using UniRx;
using UnityEngine;

namespace Survivors.Units.Player.Attack
{
    public class WeaponTimer
    {
        private readonly bool _useRealTime;
        private readonly IReadOnlyReactiveProperty<float> _attackInterval;

        private float _nextAttackTimer;
        private float _lastAttackTime;
        public event Action OnAttackReady;
  
        private bool IsAttackReady => _useRealTime ? Time.time >= _lastAttackTime + AttackInterval : _nextAttackTimer >= AttackInterval;
        private float AttackInterval => Math.Max(_attackInterval.Value, 0);
        public WeaponTimer(IReadOnlyReactiveProperty<float> attackInterval, bool useRealTime = true)
        {
            _useRealTime = useRealTime;
            _attackInterval = attackInterval;
            _lastAttackTime = Time.time - (Time.time % AttackInterval);
        }
        public void OnTick()
        {
            _nextAttackTimer += Time.deltaTime;
            if (!IsAttackReady) {
                return;
            }
            OnAttackReady?.Invoke();
            _nextAttackTimer = 0;
            _lastAttackTime = Time.time;
        }
    }
    
}