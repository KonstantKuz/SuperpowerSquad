using Feofun.Config;
using Survivors.Enemy.Config;
using Survivors.Player.Progress;

namespace Survivors.Player.Service
{
    public class PlayerProgressService
    {
        private readonly PlayerProgressRepository _repository;
        private readonly StringKeyedConfigCollection<EnemyLevelConfig> _levelConfig;
        
        public PlayerProgress Progress => _repository.Get() ?? PlayerProgress.Create();

        public PlayerProgressService(PlayerProgressRepository repository,
                                     StringKeyedConfigCollection<EnemyLevelConfig> levelConfig)
        {
            _repository = repository;
            _levelConfig = levelConfig;
        }

        public void AddWinCount()
        {
            var progress = Progress;
            progress.AddKill(_levelConfig);
            SetProgress(progress);
        }

        private void SetProgress(PlayerProgress progress)
        {
            _repository.Set(progress);
        }
    }
}