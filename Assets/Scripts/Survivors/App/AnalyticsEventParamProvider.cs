using System;
using System.Collections.Generic;
using System.Linq;
using Feofun.Config;
using Survivors.Analytics;
using Survivors.Location;
using Survivors.Player.Progress.Service;
using Survivors.Session.Config;
using Survivors.Session.Service;
using Survivors.Squad.Component;
using Survivors.Squad.Service;
using Survivors.Units.Service;
using Survivors.Upgrade;
using Survivors.Upgrade.MetaUpgrade;
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
        [Inject] private MetaUpgradeService _metaUpgradeService;
        [Inject] private World _world;
        [Inject] private ABTest.ABTest _abTest;
        
        public Dictionary<string, object> GetParams(IEnumerable<string> paramNames)
        {
            return paramNames.ToDictionary(it => it, GetValue);
        }

        private object GetValue(string paramName)
        {
            if (paramName.StartsWith(EventParams.UPGRADE)) {
                return GetUpgrade(paramName.Split(Analytics.Analytics.SEPARATOR)[1]);
            }   
            if (paramName.StartsWith(EventParams.META_UPGRADE)) {
                return GetMetaUpgrade(paramName.Split(Analytics.Analytics.SEPARATOR)[2]);
            }

            var playerProgress = _playerProgressService.Progress;
            
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
                EventParams.TOTAL_KILLS => playerProgress.Kills,
                EventParams.WINS => playerProgress.WinCount,
                EventParams.DEFEATS => playerProgress.LoseCount,
                EventParams.REVIVE_COUNT => _sessionService.Session.Revives,
                EventParams.AB_TEST_ID => _abTest.CurrentVariantId,
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
        private string GetMetaUpgrade(string upgradeId)
        {
            return $"{upgradeId}_{_metaUpgradeService.GetLevel(upgradeId)}";
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