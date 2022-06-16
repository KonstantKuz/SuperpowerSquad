using System.Collections.Generic;
using System.Linq;
using Feofun.Config;
using JetBrains.Annotations;
using SuperMaxim.Core.Extensions;
using Survivors.Analytics.Wrapper;
using Survivors.Location;
using Survivors.Player.Service;
using Survivors.Session.Config;
using Survivors.Session.Service;
using Survivors.Squad.Component;
using Survivors.Squad.Service;
using Survivors.Squad.Upgrade;
using Survivors.Units;
using Survivors.Units.Component.Health;
using Survivors.Units.Service;
using UnityEngine;
using Zenject;

namespace Survivors.Analytics
{
    [PublicAPI]
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
        [Inject] 
        private UnitService _unitService;
        [Inject] 
        private World _world;
        
        
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
            ReportEventToAllImpls(Events.TEST_EVENT, null);
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
            var enemies = GetEnemyUnits().ToList();

            var eventParams = GetLevelParams();
            eventParams[EventParams.PASS_NUMBER] = playerProgress.GetPassCount(levelConfig.Level);
            eventParams[EventParams.SQUAD_LEVEL] = _squadProgressService.Level;
            eventParams[EventParams.TIME_SINCE_LEVEL_START] = _sessionService.SessionTime;
            eventParams[EventParams.ENEMY_KILLER] = _sessionService.Kills.Value;
            eventParams[EventParams.LEVEL_RESULT] = isPlayerWinner ? "win" : "lose";
            eventParams[EventParams.TOTAL_ENEMY_HEALTH] = SumHealth(enemies);
            eventParams[EventParams.AVERAGE_ENEMY_LIFETIME] = enemies.Average(it => it.LifeTime);
            eventParams[EventParams.STAND_RATIO] = GetStandRatio();
            
            ReportEventToAllImpls(Events.LEVEL_FINISHED, eventParams);
        }

        private float GetStandRatio()
        {
            return _world.Squad.GetComponent<MovementAnalytics>().StandingTime /
                   _sessionService.SessionTime;
        }

        private static float SumHealth(List<Unit> enemies)
        {
            return enemies
                .Select(it => it.Health)
                .Where(it => it != null).
                Sum(it => it.CurrentValue.Value);
        }

        private IEnumerable<Unit> GetEnemyUnits()
        {
            return _unitService.AllUnits
                .Where(it => it.UnitType == UnitType.ENEMY)
                .Select(it => it as Unit)
                .Where(it => it != null);
        }
    }
}