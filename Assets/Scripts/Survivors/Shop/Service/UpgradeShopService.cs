using System;
using Feofun.Config;
using Survivors.Player.Wallet;
using Survivors.Shop.Config;
using Zenject;

namespace Survivors.Shop.Service
{
    public class UpgradeShopService
    {
        [Inject]
        private WalletService _walletService;

        [Inject]
        private StringKeyedConfigCollection<UpgradeProductConfig> _shopConfig;

        public bool TryBuy(string upgradeId, int level)
        {
            var product = GetProductById(upgradeId);
            return _walletService.TryRemove(product.ProductConfig.Currency, product.GetFinalCost(level));
        }
        
        public bool HasEnoughCurrency(string productId, int level)
        {
            var product = GetProductById(productId);
            return _walletService.HasEnoughCurrency(product.ProductConfig.Currency, product.GetFinalCost(level));
        }

        public IObservable<bool> HasEnoughCurrencyAsObservable(string productId, int level)
        {
            var product = GetProductById(productId);
            return _walletService.HasEnoughCurrencyAsObservable(product.ProductConfig.Currency, product.GetFinalCost(level));
        }

        public UpgradeProductConfig GetProductById(string productId) => _shopConfig.Get(productId);
    }
}