using UnityEngine;

namespace Survivors.Units.Player.Attack
{
    public class ReloadableWeaponTimer
    {
        private readonly float _attackInterval;
        private readonly float _reloadTime;
        private readonly int _clipSize;
        private readonly IAttack _attack;

        private int _currentClipSize;
        private float _lastAttackTime;
        private float _startReloadTime;
        private bool Reloaded => Time.time >= _startReloadTime + _reloadTime;
        public bool IsAttackReady => Time.time >= _lastAttackTime + _attackInterval && Reloaded;

        public ReloadableWeaponTimer(int clipSize, float attackTime, float reloadTime, IAttack attack)
        {
            _clipSize = clipSize;
            _currentClipSize = _clipSize;
            _reloadTime = reloadTime;
            _attackInterval = attackTime / clipSize;
            _attack = attack;
            attack.OnAttack += OnAttack;
        }

        public void Dispose()
        {
            _attack.OnAttack -= OnAttack;
        }

        private void OnAttack()
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
    }
}