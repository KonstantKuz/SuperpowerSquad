using System.Linq;
using Feofun.Config;
using Survivors.Enemy.Config;
using Survivors.Player.Model;
using Survivors.Player.Service;
using UnityEngine;
using Zenject;

namespace Survivors.Enemy.Service
{
    public class EnemyService
    {
        [Inject]
        private readonly StringKeyedConfigCollection<EnemyLevelConfig> _levelConfig;
        [Inject]
        private PlayerProgressService _playerProgressService;
        private PlayerProgress PlayerProgress => _playerProgressService.Progress;
        
        public EnemyLevelConfig GetLevelConfig()
        {
            return _levelConfig.Values[Mathf.Min(PlayerProgress.WinCount, _levelConfig.Count() - 1)];
        }
    }
}