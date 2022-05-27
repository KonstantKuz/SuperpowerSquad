using System;
using Feofun.Config;
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
        [Inject]
        private SquadProgressRepository _repository;
        [Inject]
        private StringKeyedConfigCollection<SquadLevelConfig> _levelConfig;
        
        private IntReactiveProperty _level;
        public IObservable<int> LevelAsObservable => _level;
        private SquadProgress Progress => _repository.Get() ?? new SquadProgress();
        public void OnWorldSetup()
        {
            _level = new IntReactiveProperty(Progress.Level);
        }
        public void AddExp(int amount)
        {
            Assert.IsTrue(amount >= 0, "Added amount of Exp should be non-negative");
            var progress = Progress;
            progress.AddExp(amount, _levelConfig);
            SetProgress(progress);
        }

        private void SetProgress(SquadProgress progress)
        {
            _repository.Set(progress);
            _level.Value = progress.Level;
        }
        private void ResetProgress()
        {
            _repository.Delete();
            _level.Value = Progress.Level;
        }
        public void OnWorldCleanUp()
        {
            ResetProgress();
        }
    }
}