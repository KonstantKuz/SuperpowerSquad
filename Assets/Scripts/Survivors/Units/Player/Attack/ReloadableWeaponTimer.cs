using System;
using UniRx;
using UnityEngine;

namespace Survivors.Units.Player.Attack
{
    public class ReloadableWeaponTimer
    {
        private readonly IReadOnlyReactiveProperty<float> AttackTime;
 
        private readonly int _clipSize;
        private readonly float _reloadTime;
    

        private int _currentClipSize;
        private float _lastAttackTime;
        private float _startReloadTime;
        private bool Reloaded => Time.time >= _startReloadTime + _reloadTime;
        public bool IsAttackReady => Time.time >= _lastAttackTime + AttackInterval && Reloaded;
        public float AttackInterval => Math.Max(AttackTime.Value, 0) / _clipSize;
        public ReloadableWeaponTimer(int clipSize, IReadOnlyReactiveProperty<float> attackTime, float reloadTime)
        {
            _clipSize = clipSize;
            _currentClipSize = _clipSize;
            AttackTime = attackTime;
            _reloadTime = reloadTime;
        }

        public void OnAttack()
        {
            _lastAttackTime = Time.time;
            --_currentClipSize;
            if (_currentClipSize <= 0) {
                Reload();
            }
        }

        private void Reload()
        {
            _startReloadTime = Time.time;
            _currentClipSize = _clipSize;
        }

        public void CancelLastTimer()
        {
            _lastAttackTime = Time.time - AttackInterval;
            if (_currentClipSize >= _clipSize)
            {
                _startReloadTime = Time.time;
                _currentClipSize = 1;
            }
            else
            {
                _currentClipSize++;
            }
        }
    }
}