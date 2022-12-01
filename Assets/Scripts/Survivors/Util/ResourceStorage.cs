using System;
using System.Collections.Generic;
using System.Linq;
using Feofun.Repository;
using UniRx;
using UnityEngine.Assertions;

namespace Survivors.Util
{
    public class ResourceStorage
    {
        
        private readonly ISingleModelRepository<Dictionary<string, int>> _repository;

        private readonly Dictionary<string, int> _initialResources;
        
        private Dictionary<string, ReactiveProperty<int>> _resources;
        
        public ResourceStorage(ISingleModelRepository<Dictionary<string, int>> repository, 
            Dictionary<string, int> initialResources)
        {
            _repository = repository;
            _initialResources = initialResources;
            Load();
        }

        public IReactiveProperty<int> GetAsObservable(string resource) => _resources[resource];

        public int Get(string resource) => _resources[resource].Value;

        public void Add(string resource, int amount)
        {
            Assert.IsTrue(amount >= 0, "Should add non-negative amount of resource");
            TryChange(resource, amount);
        }
        public void Remove(string resource, int amount)
        {
            if (!TryRemove(resource, amount)) {
                throw new Exception($"Resource removing  error, {resource} can't be < initial value:= {_initialResources[resource]}");
            }
        }
        public bool TryRemove(string resource, int amount)
        {
            Assert.IsTrue(amount >= 0, "Should remove non-negative amount of resource");
            return TryChange(resource, -amount);
        }
        
        public void Set(string resource, int amount)
        {
            Assert.IsTrue(amount >= _initialResources[resource], $"Should add >= {_initialResources[resource]} amount of resource");
            _resources[resource].Value = amount;
            Save();
        }

        public void Reset()
        {
            foreach (var pair in _initialResources) {
                _resources[pair.Key].Value = pair.Value;
            }
            Save();
        }
        
        private bool TryChange(string resource, int delta)
        {
            var amount = _resources[resource].Value;
            if (amount + delta < _initialResources[resource]) return false;
            _resources[resource].Value = amount + delta;
            Save();
            return true;
        }

        private void Load()
        {
            var data = _repository.Get() ?? _initialResources;
            _resources = data.ToDictionary(pair => pair.Key, pair => new ReactiveProperty<int>(pair.Value));
        }
        private void Save()
        {
            var data = _resources.ToList().ToDictionary(pair => pair.Key.ToString(), pair => pair.Value.Value);
            _repository.Set(data);
        }

    }
}