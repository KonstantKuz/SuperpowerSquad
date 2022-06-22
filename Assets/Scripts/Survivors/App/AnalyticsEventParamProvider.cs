using System;
using System.Collections.Generic;
using System.Linq;
using Feofun.Config;
using Survivors.Analytics;
using Survivors.Location;
using Survivors.Player.Service;
using Survivors.Session.Config;
using Survivors.Session.Service;
using Survivors.Squad.Component;
using Survivors.Squad.Service;
using Survivors.Squad.Upgrade;
using Survivors.Units.Service;
using UnityEngine;
using UnityEngine.Assertions;
using Zenject;

namespace Survivors.App
{
    public class AnalyticsEventParamProvider: IEventParamProvider
    {
        [Inject] private SessionService _sessionService;
        [Inject] private PlayerProgressService _playerProgressService;
        [Inject] private StringKeyedConfigCollection<LevelMissionConfig> _levelsConfig;
        [Inject] private SquadProgressService _squadProgressService;
        [Inject] private SquadUpgradeRepository _squadUpgradeRepository;
        [Inject] private UnitService _unitService;
        [Inject] private World _world;
        
        
        public Dictionary<string, object> GetParams(IEnumerable<string> paramNames)
        {
            return paramNames.ToDictionary(it => it, it => GetValue(it));
        }

        private object GetValue(string paramName)
        {
            if (paramName.StartsWith(EventParams.UPGRADE))
            {
                return GetUpgrade(paramName.Split(Analytics.Analytics.Separator)[1]);
            }
            return paramName switch
            {
                EventParams.LEVEL_ID => _sessionService.LevelId,
                EventParams.LEVEL_NUMBER => GetLevelNumber(),
                EventParams.LEVEL_LOOP => GetLevelLoop(),
                EventParams.SQUAD_LEVEL => _squadProgressService.Level.Value,
                EventParams.ENEMY_KILLED => _sessionService.Kills.Value,
                EventParams.TIME_SINCE_LEVEL_START => _sessionService.SessionTime,
                EventParams.PASS_NUMBER => GetPassNumber(),
                EventParams.TOTAL_ENEMY_HEALTH => GetTotalEnemyHealth(),
                EventParams.AVERAGE_ENEMY_LIFETIME => GetAverageEnemyLifetime(),
                EventParams.STAND_RATIO => GetStandRatio(),
                
                _ => throw new ArgumentOutOfRangeException(nameof(paramName), paramName, $"Unsupported analytics parameter {paramName}")
            };
        }

        private int GetLevelNumber() 
        {
            var playerProgress = _playerProgressService.Progress;
            return playerProgress.LevelNumber + 1;
        }

        private int GetLevelLoop()
        {
            var playerProgress = _playerProgressService.Progress;
            return Mathf.Max(0, playerProgress.LevelNumber - _levelsConfig.Keys.Count);
        }

        private string GetUpgrade(string upgradeBranch)
        {
            return $"{upgradeBranch}_{_squadUpgradeRepository.Get().GetLevel(upgradeBranch)}";
        }

        private int GetPassNumber()
        {
            var playerProgress = _playerProgressService.Progress;
            var levelConfig = _levelsConfig.Values[_sessionService.LevelId];
            return playerProgress.GetPassCount(levelConfig.Level);
        }
        
        private float GetTotalEnemyHealth()
        {
            var enemies = _unitService.GetEnemyUnits().ToList();
            return enemies
                .Select(it => it.Health)
                .Where(it => it != null).
                Sum(it => it.CurrentValue.Value);
        }

        private float GetAverageEnemyLifetime()
        {
            var enemies = _unitService.GetEnemyUnits().ToList();
            return enemies.Average(it => it.LifeTime);
        }
        
        private float GetStandRatio()
        {
            Assert.IsNotNull(_world.Squad, "Should call this method only inside game session");
            return _world.Squad.GetComponent<MovementAnalytics>().StandingTime /
                   _sessionService.SessionTime;
        }
    }
}