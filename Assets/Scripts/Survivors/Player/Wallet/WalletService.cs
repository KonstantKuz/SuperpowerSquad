using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine.Assertions;

namespace Survivors.Player.Wallet
{
    public class WalletService
    {
        private readonly WalletRepository _repository;
        
        private readonly Dictionary<Currency, ReactiveProperty<int>> _wallet;

        public IObservable<Unit> AnyMoneyObservable =>
                Enum.GetValues(typeof(Currency)).Cast<Currency>().Select(it => GetMoneyAsObservable(it).Select(it => new Unit())).Merge();

     
        public WalletService(WalletRepository repository)
        {
            _repository = repository;
            _wallet = Enum.GetValues(typeof(Currency))
                          .OfType<Currency>()
                          .Select(currency => (currency, new ReactiveProperty<int>(0)))
                          .ToDictionary(pair => pair.currency, pair => pair.Item2);
            Load();
        }

        public IObservable<int> GetMoneyAsObservable(Currency currency) => _wallet[currency];

        public IObservable<bool> HasEnoughCurrencyAsObservable(Currency currency, int count) =>
                GetMoneyAsObservable(currency).Select(value => value >= count);

        public int GetMoney(Currency currency) => _wallet[currency].Value;

        public void Add(Currency currency, int amount)
        {
            Assert.IsTrue(amount >= 0, "Should add non-negative amount of currency");
            TryChange(currency, amount);
        }

        public bool TryRemove(Currency currency, int amount)
        {
            Assert.IsTrue(amount >= 0, "Should remove non-negative amount of currency");
            return TryChange(currency, -amount);
        }
        
        public void Set(Currency currency, int amount)
        {
            Assert.IsTrue(amount >= 0, "Should add non-negative amount of currency");
            _wallet[currency].Value = amount;
            Save();
        }
        
        public bool HasEnoughCurrency(Currency currency, int count)
        {
            return _wallet[currency].Value >= count;
        }
        public void ResetWallet()
        {
            foreach (var key in _wallet.Keys.ToList()) {
                _wallet[key].Value = 0;
            }
            Save();
        }
        public void ResetCurrency(Currency currency)
        {
            _wallet[currency].Value = 0;
            Save();
        }

        private bool TryChange(Currency currency, int delta)
        {
            var amount = _wallet[currency].Value;
            if (amount + delta < 0) return false;
            _wallet[currency].Value = amount + delta;
            Save();
            return true;
        }

        private void Load()
        {
            var data = _repository.Get() ?? new Dictionary<string, int>();
            foreach (var pair in data) {
                if (Enum.TryParse(pair.Key, out Currency currency)) {
                    _wallet[currency].SetValueAndForceNotify(pair.Value);
                }
            }
        }
        private void Save()
        {
            var data = _wallet.ToList().ToDictionary(pair => pair.Key.ToString(), pair => pair.Value.Value);
            _repository.Set(data);
        }
    }
}