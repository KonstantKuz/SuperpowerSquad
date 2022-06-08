using System;
using Feofun.Config;
using Survivors.Enemy.Config;
using Survivors.Player.Progress;
using Survivors.Player.Service;
using UnityEngine;
using Zenject;

namespace Survivors.Enemy.Service
{
    public class EnemyService
    {
        [Inject]
        private PlayerProgressService _playerProgressService;

        public int GetEnemyLevelConfig()
        {
            return gameMode switch {
                    GameMode.Battle => _battleEnemies.GetSquadByIndex(Mathf.Min(PlayerProgress.WinCount, _battleEnemies.SquadCount - 1)).Id,
                    GameMode.HyperCasual => _battleEnemies.GetSquadByIndex(PlayerProgress.HyperCasualProgress % _battleEnemies.SquadCount).Id,
                    _ => throw new ArgumentOutOfRangeException(nameof(gameMode), gameMode, null)
            };
        }
        public bool IsMaxLevel(StringKeyedConfigCollection<EnemyLevelConfig> levels) => WinCount > levels.Values.Count;
        public int MaxKillsForCurrentLevel(StringKeyedConfigCollection<EnemyLevelConfig> levels) => CurrentLevelConfig(levels).KillCount;

        public EnemyLevelConfig CurrentLevelConfig(StringKeyedConfigCollection<EnemyLevelConfig> levels) =>
                levels.Values[Math.Min(levels.Values.Count - 1, WinCount - 1)];

        private PlayerProgress PlayerProgress => _playerProgressService.Progress;
    }
}