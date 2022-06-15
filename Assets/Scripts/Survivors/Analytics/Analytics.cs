using System.Collections.Generic;
using Feofun.Config;
using SuperMaxim.Core.Extensions;
using Survivors.Analytics.Wrapper;
using Survivors.Player.Service;
using Survivors.Session.Config;
using Survivors.Session.Service;
using Survivors.Squad.Service;
using Survivors.Squad.Upgrade;
using UnityEngine;
using Zenject;

namespace Survivors.Analytics
{
    public class Analytics
    {
        [Inject] 
        private StringKeyedConfigCollection<LevelMissionConfig> _levelsConfig;
        [Inject] 
        private PlayerProgressService _playerProgressService;
        [Inject]
        private SquadProgressService _squadProgressService;
        [Inject] 
        private SquadUpgradeRepository _squadUpgradeRepository;
        [Inject] 
        private SessionService _sessionService;
        
        
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

        public void ReportLevelStart(int levelId)
        {
            var playerProgress = _playerProgressService.Progress;
            var levelConfig = _levelsConfig.Values[levelId];

            var eventParams = GetLevelParams();
            eventParams[EventParams.PASS_NUMBER] = playerProgress.GetPassCount(levelConfig.Level);
            
            ReportEventToAllImpls(Events.LEVEL_START, eventParams);
        }

        private Dictionary<string, object> GetLevelParams()
        {
            var playerProgress = _playerProgressService.Progress;
            return new Dictionary<string, object>
            {
                {EventParams.LEVEL_ID, _sessionService.LevelId},
                {EventParams.LEVEL_NUMBER, playerProgress.LevelNumber + 1},
                {EventParams.LEVEL_LOOP, Mathf.Max(0, playerProgress.LevelNumber - _levelsConfig.Keys.Count)},
            };
        }

        private void ReportEventToAllImpls(string eventName, Dictionary<string, object> eventParams)
        {
            foreach (var impl in _impls)
            {
                impl.ReportEventWithParams(eventName, eventParams);
            }
        }

        public void ReportLevelUp(string upgradeBranch)
        {
            var eventParams = GetLevelParams();
            eventParams[EventParams.SQUAD_LEVEL] = _squadProgressService.Level;
            eventParams[EventParams.UPGRADE] =
                $"{upgradeBranch}_{_squadUpgradeRepository.Get().GetLevel(upgradeBranch)}";
            eventParams[EventParams.ENEMY_KILLER] = _sessionService.Kills.Value;
            eventParams[EventParams.TIME_SINCE_LEVEL_START] = _sessionService.SessionTime;
            
            ReportEventToAllImpls(Events.LEVEL_UP, eventParams);
        }

        public void ReportLevelFinished(bool isPlayerWinner)
        {
            var playerProgress = _playerProgressService.Progress;
            var levelConfig = _levelsConfig.Values[_sessionService.LevelId];

            var eventParams = GetLevelParams();
            eventParams[EventParams.PASS_NUMBER] = playerProgress.GetPassCount(levelConfig.Level);
            eventParams[EventParams.SQUAD_LEVEL] = _squadProgressService.Level;
            eventParams[EventParams.TIME_SINCE_LEVEL_START] = _sessionService.SessionTime;
            eventParams[EventParams.ENEMY_KILLER] = _sessionService.Kills.Value;
            eventParams[EventParams.LEVEL_RESULT] = isPlayerWinner ? "win" : "lose";
            
            ReportEventToAllImpls(Events.LEVEL_FINISHED, eventParams);
        }
    }
}