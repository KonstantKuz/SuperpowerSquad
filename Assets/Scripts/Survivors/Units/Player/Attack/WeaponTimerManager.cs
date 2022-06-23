using System;
using System.Collections.Generic;
using Feofun.Components;
using SuperMaxim.Core.Extensions;
using Survivors.Units.Model;
using UnityEngine;

namespace Survivors.Units.Player.Attack
{
    public class WeaponTimerManager : MonoBehaviour, IWeaponTimerManager, IInitializable<Squad.Squad>
    {
        private Dictionary<string, WeaponTimer> _timers;
        
        public void Init(Squad.Squad owner)
        {
            _timers = new Dictionary<string, WeaponTimer>();
        }
        public void Subscribe(string weaponId, IAttackModel attackModel, Action onAttackReady)
        {
            if (!_timers.ContainsKey(weaponId)) {
                AddTimer(weaponId, attackModel);
            } 
            _timers[weaponId].OnAttackReady += onAttackReady;
        }
        public void Unsubscribe(string weaponId, Action onAttackReady)
        {
            if (_timers == null || !_timers.ContainsKey(weaponId)) {
                return;
            }
            _timers[weaponId].OnAttackReady -= onAttackReady;
        }
        private void AddTimer(string unitTypeId, IAttackModel attackModel)
        {
            _timers[unitTypeId] = new WeaponTimer(attackModel.AttackInterval);;
        }
        private void Update()
        {
            _timers?.Values.ForEach(it => it.OnTick());
        }
    }
}