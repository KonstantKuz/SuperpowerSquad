using System.Collections.Generic;
using SuperMaxim.Core.Extensions;
using Survivors.Location;
using UnityEngine;

namespace Survivors.Units.Player.Attack
{
    public class WeaponTimerManager : MonoBehaviour, IWorldScope
    {
        private Dictionary<string, ReloadableWeaponTimer> _timers = new Dictionary<string, ReloadableWeaponTimer>();
        
        public ReloadableWeaponTimer AddTimer(string id, ReloadableWeaponTimer timer)
        {
            if (!_timers.ContainsKey(id)) { 
                _timers[id] = timer;
            }
            return _timers[id];
        }

        private void Update()
        {
            _timers?.Values.ForEach(it => it.OnTick());
        }

        public void OnWorldSetup()
        {
            _timers = new Dictionary<string, ReloadableWeaponTimer>();
        }

        public void OnWorldCleanUp()
        {
            _timers = null;
        }
    }
}