using Feofun.Config;
using Survivors.Squad.Config;
using Survivors.Squad.Progress;
using UnityEngine;
using UnityEngine.Assertions;

namespace Survivors.Squad.Service
{
    public class SquadUpgradeService
    {
        private readonly SquadProgressRepository _repository;
        private readonly StringKeyedConfigCollection<SquadLevelConfig> _levelStringKeyedConfig;

        public SquadProgress Progress => _repository.Get() ?? new SquadProgress();
        
        public SquadUpgradeService(SquadProgressRepository repository, 
                                     StringKeyedConfigCollection<SquadLevelConfig> levelStringKeyedConfig)
        {
            _repository = repository;
            _levelStringKeyedConfig = levelStringKeyedConfig;
        }

        public void ResetProgress()
        {
            _repository.Delete();
        }

        public void AddExp(int amount)
        {
            Assert.IsTrue(amount >= 0, "Added amount of Exp should be non-negative");
            var progress = Progress;
            progress.AddExp(amount, _levelStringKeyedConfig);
            SetProgress(progress);
            Debug.Log($"Update progress! Level == {progress.Level} Exp == {progress.Exp}");
        }

        private void SetProgress(SquadProgress progress)
        {
            _repository.Set(progress);
        }
    }
}