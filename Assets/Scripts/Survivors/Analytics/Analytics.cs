using System.Collections.Generic;
using Feofun.Config;
using SuperMaxim.Core.Extensions;
using Survivors.Analytics.Wrapper;
using Survivors.Player.Model;
using Survivors.Session.Config;
using UnityEngine;
using Zenject;

namespace Survivors.Analytics
{
    public class Analytics
    {
        [Inject] private readonly StringKeyedConfigCollection<LevelMissionConfig> _levelsConfig; 
        
        private readonly ICollection<IAnalyticsImpl> _impls;
        
        public Analytics(ICollection<IAnalyticsImpl> impls)
        {
            _impls = impls;
        }

        public void Init()
        {
            Debug.Log("Initializing Analytics");
            foreach (var impl in _impls)
            {
                impl.Init();
            }
        }
        public void ReportTest()
        {
            _impls.ForEach(it => it.ReportTest());
        }

        public void ReportLevelStart(PlayerProgress playerProgress, LevelMissionConfig levelMissionConfig)
        {
            var eventParams = new Dictionary<string, object>
            {
                {EventParams.LEVEL_ID, levelMissionConfig.Id},
                {EventParams.LEVEL_NUMBER, playerProgress.LevelNumber + 1},
                {EventParams.LEVEL_LOOP, Mathf.Max(0, playerProgress.LevelNumber - _levelsConfig.Keys.Count)}
            }; 
            foreach (var impl in _impls)
            {
                impl.ReportEventWithParams(Events.LEVEL_START, eventParams);
            }
        }
    }
}