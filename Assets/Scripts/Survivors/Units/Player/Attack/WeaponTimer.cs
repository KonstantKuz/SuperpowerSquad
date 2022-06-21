using System;
using UniRx;
using UnityEngine;

namespace Survivors.Units.Player.Attack
{
    public class WeaponTimer
    {
        private readonly IReadOnlyReactiveProperty<float> _attackTime;
        
        private float _lastAttackTime;
        public event Action OnReload;
  
        public bool IsAttackReady => Time.time >= _lastAttackTime + AttackInterval;
        public float AttackInterval => Math.Max(_attackTime.Value, 0);
        public WeaponTimer(IReadOnlyReactiveProperty<float> attackTime)
        {
            _attackTime = attackTime;
            _lastAttackTime = Time.time - (Time.time % AttackInterval);
        }
        public void OnTick()
        {
            if (!IsAttackReady) {
                return;
            }
            OnReload?.Invoke();
            _lastAttackTime = Time.time;
        }
    }
}