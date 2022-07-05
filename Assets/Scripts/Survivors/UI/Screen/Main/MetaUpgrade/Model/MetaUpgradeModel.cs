using System;
using System.Collections.Generic;
using System.Linq;
using Feofun.Config;
using Feofun.Localization;
using Survivors.Modifiers.Config;
using Survivors.Shop.Service;
using Survivors.Squad.Upgrade;
using Survivors.UI.Components.PriceButton;
using Survivors.Util;

namespace Survivors.UI.Screen.Main.MetaUpgrade.Model
{
    public class MetaUpgradeModel
    {
        private const string LEVEL_LOCALIZATION_ID = "lvl";

        private readonly MetaUpgradeService _upgradeService;
        private readonly UpgradeShopService _shopService;

        private readonly List<MetaUpgradeItemModel> _upgrades;

        public IReadOnlyCollection<MetaUpgradeItemModel> Upgrades => _upgrades;

        public MetaUpgradeModel(StringKeyedConfigCollection<ParameterUpgradeConfig> modifierConfigs,
                                MetaUpgradeService upgradeService,
                                UpgradeShopService shopService,
                                Action<string> onUpgrade)
        {
            _upgradeService = upgradeService;
            _shopService = shopService;
            _upgrades = modifierConfigs.Select(id => BuildUpgradeItemModel(id, onUpgrade)).ToList();
        }

        private MetaUpgradeItemModel BuildUpgradeItemModel(ParameterUpgradeConfig upgradeConfig, Action<string> onUpgrade)
        {
            var id = upgradeConfig.Id;
            var nextLevel = _upgradeService.GetLevel(id) + 1;
            return new MetaUpgradeItemModel() {
                    Id = id,
                    Name = LocalizableText.Create(id),
                    Level = LocalizableText.Create(LEVEL_LOCALIZATION_ID, nextLevel),
                    PriceModel = CreatePriceModel(id, nextLevel),
                    OnClick = () => onUpgrade?.Invoke(upgradeConfig.Id),
            };
        }

        private PriceButtonModel CreatePriceModel(string upgradeId, int nextLevel)
        {
            var productConfig = _shopService.GetProductById(upgradeId);
            var price = productConfig.GetFinalCost(nextLevel);
            return new PriceButtonModel() {
                    Price = price,
                    PriceText = price.ToString(),
                    Enabled = true,
                    CanBuy = _shopService.HasEnoughCurrencyAsObservable(upgradeId, nextLevel),
                    CurrencyIconPath = IconPath.GetCurrency(productConfig.ProductConfig.Currency.ToString())
            };
        }
    }
}