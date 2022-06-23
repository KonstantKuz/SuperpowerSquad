using System;
using UniRx;
using UnityEngine;

namespace Survivors.Units.Player.Attack
{
    public class WeaponTimer
    {
        private readonly IReadOnlyReactiveProperty<float> _attackInterval;

        private float _timer;
        public event Action OnAttackReady;
  
        private bool IsAttackReady => _timer >= AttackInterval;
        private float AttackInterval => Math.Max(_attackInterval.Value, 0);
        public WeaponTimer(IReadOnlyReactiveProperty<float> attackInterval)
        {
            _attackInterval = attackInterval;
        }

        public void OnTick()
        {
            _timer += Time.deltaTime;
            if (!IsAttackReady) {
                return;
            }
            OnAttackReady?.Invoke();
            _timer = 0f;
        }
    }
}