using UnityEngine;

namespace Survivors.Units.Player.Attack
{
    public class ReloadableWeaponTimer
    {
        private readonly float _attackInterval;
 
        private readonly float _reloadTime;
        private readonly int _clipSize;

        private int _currentClipSize;
        private float _lastAttackTime;
        private float _startReloadTime;
        private bool Reloaded => Time.time >= _startReloadTime + _reloadTime;
        public bool IsAttackReady => Time.time >= _lastAttackTime + _attackInterval && Reloaded;
        public float AttackInterval => _attackInterval;
        public ReloadableWeaponTimer(int clipSize, float attackTime, float reloadTime)
        {
            _clipSize = clipSize;
            _currentClipSize = _clipSize;
            _reloadTime = reloadTime;
            _attackInterval = attackTime / clipSize;
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
            _lastAttackTime = Time.time - _attackInterval;
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