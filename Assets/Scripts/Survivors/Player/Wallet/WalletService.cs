using System;
using System.Linq;
using Survivors.Util.Storage;
using UniRx;

namespace Survivors.Player.Wallet
{
    public class WalletService : IResourceStorage<Currency, int>
    {
        public IObservable<Unit> AnyMoneyObservable =>
            Enum.GetValues(typeof(Currency)).Cast<Currency>().Select(it => GetAsObservable(it)
                .Select(it => new Unit())).Merge();

        private readonly IResourceStorage<string, int> _resourceStorage;

        public WalletService()
        {
            var initialResources = Enum.GetValues(typeof(Currency))
                .OfType<Currency>()
                .ToDictionary(currency => currency.ToString(), currency => 0);
            _resourceStorage = new ResourceStorage(new WalletRepository(), initialResources);
        }

        public bool HasEnoughCurrency(Currency currency, int count) => Get(currency) >= count;
        public IObservable<bool> HasEnoughCurrencyAsObservable(Currency currency, int count) =>
            GetAsObservable(currency).Select(value => value >= count);

        public IReactiveProperty<int> GetAsObservable(Currency currency) =>
            _resourceStorage.GetAsObservable(currency.ToString());

        public int Get(Currency currency) => _resourceStorage.Get(currency.ToString());

        public void Add(Currency currency, int amount) => _resourceStorage.Add(currency.ToString(), amount);

        public void Remove(Currency currency, int amount) => _resourceStorage.Remove(currency.ToString(), amount);

        public bool TryRemove(Currency currency, int amount) => _resourceStorage.TryRemove(currency.ToString(), amount);

        public void Set(Currency currency, int amount) => _resourceStorage.Set(currency.ToString(), amount);

        public void Reset() => _resourceStorage.Reset();
    }
}