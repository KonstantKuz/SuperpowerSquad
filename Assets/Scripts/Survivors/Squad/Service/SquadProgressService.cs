using System;
using Feofun.Config;
using JetBrains.Annotations;
using SuperMaxim.Messaging;
using Survivors.App.Config;
using Survivors.Location;
using Survivors.Session.Messages;
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
        private readonly IntReactiveProperty _exp = new IntReactiveProperty(0);
        
        [Inject]
        private SquadProgressRepository _repository;
        [Inject]
        private StringKeyedConfigCollection<SquadLevelConfig> _levelConfig;
        [Inject] 
        private ConstantsConfig _constantsConfig;
        [Inject]
        private IMessenger _messenger;    
        
        public IReadOnlyReactiveProperty<int> Level => _level;    
        public IObservable<int> Exp => _exp;
        private SquadProgress Progress => _repository.Require();
        
        [CanBeNull]
        public SquadLevelConfig CurrentLevelConfig => _repository.Exists() ? Progress.CurrentLevelConfig(_levelConfig) : null; 
        
        private int ExpToNextLevel => Progress.MaxExpForCurrentLevel(_levelConfig) - Progress.Exp;
        public void OnWorldSetup()
        {
            SetProgress(SquadProgress.Create());
            if (_constantsConfig.LevelUpBetweenWaves)
            {
                _messenger.Subscribe<WaveClearedMessage>(OnWaveCleared);
            }
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
            _exp.Value = progress.Exp;
        }
        private void ResetProgress()
        {
            _repository.Delete();
            _level.Value = SquadProgress.DEFAULT_LEVEL;
            _exp.Value = 0;
        }
        public void OnWorldCleanUp()
        {
            if (_constantsConfig.LevelUpBetweenWaves)
            {
                _messenger.Unsubscribe<WaveClearedMessage>(OnWaveCleared);
            }            
            ResetProgress();
        }
        
        private void OnWaveCleared(WaveClearedMessage msg)
        {
            if (msg.IsLastWave) return;
            
            var progress = Progress;
            if (progress.IsMaxLevel(_levelConfig)) return;

            progress.Level++;
            SetProgress(progress);
        }   
    }
}