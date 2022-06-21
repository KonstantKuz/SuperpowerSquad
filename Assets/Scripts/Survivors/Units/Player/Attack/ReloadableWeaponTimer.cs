using System;
using UniRx;
using UnityEngine;

namespace Survivors.Units.Player.Attack
{
    public class ReloadableWeaponTimer
    {
        private readonly IReadOnlyReactiveProperty<float> AttackTime;
        
        public event Action OnReload;
        
        private readonly int _clipSize;
        private readonly float _reloadTime;
        
        private float _lastAttackTime;
        private float _startReloadTime;

        public bool IsAttackReady => Time.time >= _lastAttackTime + AttackInterval;
        public float AttackInterval => Math.Max(AttackTime.Value, 0) / _clipSize;
        public ReloadableWeaponTimer(int clipSize, IReadOnlyReactiveProperty<float> attackTime, float reloadTime)
        {
            _clipSize = clipSize;
            AttackTime = attackTime;
            _reloadTime = reloadTime;
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