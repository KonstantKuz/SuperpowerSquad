using System.Collections.Generic;
using SuperMaxim.Core.Extensions;
using Survivors.Location;
using Survivors.Units.Model;
using UnityEngine;

namespace Survivors.Units.Player.Attack
{
    public class WeaponTimerManager : MonoBehaviour, IWorldScope
    {
        private Dictionary<string, WeaponTimer> _timers;
        public void OnWorldSetup()
        {
            _timers = new Dictionary<string, WeaponTimer>();
        }
        public WeaponTimer GetOrCreateTimer(string unitTypeId, IAttackModel attackModel)
        {
            if (!_timers.ContainsKey(unitTypeId)) {
                AddTimer(unitTypeId, attackModel);
            }
            return _timers[unitTypeId];
        }
        private void AddTimer(string unitTypeId, IAttackModel attackModel)
        {
            _timers[unitTypeId] = new WeaponTimer(attackModel.AttackTime);;
        }
        private void Update()
        {
            _timers?.Values.ForEach(it => it.OnTick());
        }
     
        public void OnWorldCleanUp()
        {
            _timers = null;
        }
    }
}