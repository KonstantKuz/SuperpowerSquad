using System;
using Feofun.Config;
using Survivors.Location;
using Survivors.Session;
using Survivors.Squad.Config;
using Survivors.Squad.Progress;
using UniRx;
using UnityEngine.Assertions;
using Zenject;

namespace Survivors.Squad.Service
{
    public class SquadProgressService : IWorldScope
    {
        private readonly IntReactiveProperty _level = new IntReactiveProperty(SquadProgress.DEFAULT_LEVEL);
        
        [Inject]
        private SquadProgressRepository _repository;
        [Inject]
        private StringKeyedConfigCollection<SquadLevelConfig> _levelConfig;
        public IObservable<int> Level => _level;
        private SquadProgress Progress => _repository.Require();
        private int ExpToNextLevel => Progress.MaxExpForCurrentLevel(_levelConfig) - Progress.Exp;
        public void OnWorldSetup()
        {
            _repository.Set(SquadProgress.Create());
            _level.Value = Progress.Level;
        }
        public void AddExp(int amount)
        {
            Assert.IsTrue(amount >= 0, "Added amount of Exp should be non-negative");
            var progress = Progress;
            progress.AddExp(amount, _levelConfig);
            SetProgress(progress);
        }

        public void IncreaseLevel()
        {
            AddExp(ExpToNextLevel);
        }

        private void SetProgress(SquadProgress progress)
        {
            _repository.Set(progress);
            _level.Value = progress.Level;
        }
        private void ResetProgress()
        {
            _repository.Delete();
            _level.Value = SquadProgress.DEFAULT_LEVEL;
        }
        public void OnWorldCleanUp()
        {
            ResetProgress();
        }
    }
}