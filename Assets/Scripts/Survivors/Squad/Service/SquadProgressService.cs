using System;
using System.Collections.Generic;
using Feofun.Config;
using Survivors.Location;
using Survivors.Squad.Config;
using Survivors.Squad.Data;
using Survivors.Util;
using UniRx;
using UnityEngine.Assertions;
using Zenject;

namespace Survivors.Squad.Service
{
    public class SquadProgressService :  IWorldScope
    {
        private const int DEFAULT_LEVEL = 1;
        
        private readonly ResourceStorage _resourceStorage;
        
        [Inject] 
        private StringKeyedConfigCollection<SquadLevelConfig> _levelConfig;

        
        private bool IsMaxCurrentLevel => Get(SquadProgressType.Level) > _levelConfig.Values.Count;
        
        private int MaxExpForCurrentLevel => CurrentLevelConfig.ExpToNextLevel;
        private int ExpToNextLevel => MaxExpForCurrentLevel - Get(SquadProgressType.Exp);
        
        public SquadLevelConfig CurrentLevelConfig =>
            _levelConfig.Values[Math.Min(_levelConfig.Values.Count - 1,  Get(SquadProgressType.Level) - 1)];
        
        public SquadProgressService()
        {
            _resourceStorage = new ResourceStorage(new SquadProgressRepository(), new Dictionary<string, int>()
            {
                {SquadProgressType.Level.ToString(), DEFAULT_LEVEL},
                {SquadProgressType.Exp.ToString(), 0},
                {SquadProgressType.Token.ToString(), 0},
            });
        }
        
        public IReactiveProperty<int> GetAsObservable(SquadProgressType progressType) =>
            _resourceStorage.GetAsObservable(progressType.ToString());

        public int Get(SquadProgressType progressType) => _resourceStorage.Get(progressType.ToString());

        public void AddExp(int amount)
        {
            Assert.IsTrue(amount >= 0, $"Added amount of exp should be non-negative");
            RecalculateExp(amount);
        }  
        public void AddToken(int amount) => AddToResource(SquadProgressType.Token, amount);
        public void RemoveToken(int amount) => RemoveFromResource(SquadProgressType.Token, amount);

        public void IncreaseLevel()
        {
            AddExp(ExpToNextLevel);
        }
        
        private void RecalculateExp(int amount)
        {
            AddToResource(SquadProgressType.Exp, amount);
            while (Get(SquadProgressType.Exp) >= MaxExpForCurrentLevel && !IsMaxCurrentLevel)
            {
                RemoveFromResource(SquadProgressType.Exp, MaxExpForCurrentLevel);
                AddToResource(SquadProgressType.Level, 1);
            }
            
        }
        private void AddToResource(SquadProgressType progressType, int amount) => _resourceStorage.Add(progressType.ToString(), amount);
        private void RemoveFromResource(SquadProgressType progressType, int amount) => _resourceStorage.Remove(progressType.ToString(), amount);
        
        public void OnWorldSetup() { }

        public void OnWorldCleanUp()
        {
            ResetProgress();
        }
        
        private void ResetProgress()
        {
            _resourceStorage.Reset();
        }

    }
}