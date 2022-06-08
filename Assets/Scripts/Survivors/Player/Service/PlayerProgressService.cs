using System;
using Feofun.Config;
using Survivors.Player.Config;
using Survivors.Player.Progress;
using Survivors.Session;
using UniRx;

namespace Survivors.Player.Service
{
    public class PlayerProgressService : IWorldScope
    {
        private readonly PlayerProgressRepository _repository;
        private readonly StringKeyedConfigCollection<PlayerLevelConfig> _levelConfig;
        
        private IntReactiveProperty _kills; 
        private IntReactiveProperty _level;

        public IReadOnlyReactiveProperty<int> Kills => _kills;
        public IReadOnlyReactiveProperty<int> Level => _level;
        public PlayerProgress Progress => _repository.Get() ?? PlayerProgress.Create();

        public PlayerProgressService(PlayerProgressRepository repository,
                                     StringKeyedConfigCollection<PlayerLevelConfig> levelConfig)
        {
            _repository = repository;
            _levelConfig = levelConfig;
            _kills = new IntReactiveProperty(Progress.Kills);
            _level = new IntReactiveProperty(Progress.Level);
        }
        public void OnWorldSetup()
        {
            var progress = Progress;
            progress.ResetKills();
            SetProgress(progress);
        }

        public void OnWorldCleanUp()
        {
            throw new NotImplementedException();
        }
        
        public void AddKill()
        {
            var progress = Progress;
            progress.AddKill(_levelConfig);
            SetProgress(progress);
        }

        private void SetProgress(PlayerProgress progress)
        {
            _repository.Set(progress);
            _level.Value = progress.Level;
            _kills.Value = progress.Kills;
        }


    }
}