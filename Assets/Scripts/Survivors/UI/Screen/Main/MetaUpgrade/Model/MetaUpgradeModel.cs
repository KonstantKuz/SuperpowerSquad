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
using UniRx;

namespace Survivors.UI.Screen.Main.MetaUpgrade.Model
{
    public class MetaUpgradeModel
    {
        private const string LEVEL_LOCALIZATION_ID = "lvl";
        private const string UPGRADE_NAME_LOCALIZATION_PREFIX = "Meta";

        private readonly StringKeyedConfigCollection<ParameterUpgradeConfig> _modifierConfigs;
        private readonly MetaUpgradeService _upgradeService;
        private readonly UpgradeShopService _shopService;

        private readonly Action<string> _onUpgrade;
        private readonly List<ReactiveProperty<MetaUpgradeItemModel>> _upgrades;

        public IReadOnlyCollection<IObservable<MetaUpgradeItemModel>> Upgrades => _upgrades;

        public MetaUpgradeModel(StringKeyedConfigCollection<ParameterUpgradeConfig> modifierConfigs,
                                MetaUpgradeService upgradeService,
                                UpgradeShopService shopService,
                                Action<string> onUpgrade)
        {
            _upgradeService = upgradeService;
            _shopService = shopService;
            _modifierConfigs = modifierConfigs;
            _onUpgrade = onUpgrade;
            _upgrades = modifierConfigs.Select(id => new ReactiveProperty<MetaUpgradeItemModel>(BuildUpgradeItemModel(id))).ToList();
        }

        public void RebuildUpgradeItem(string upgradeId)
        {
            var property = _upgrades.First(it => it.Value.Id == upgradeId);
            property.SetValueAndForceNotify(BuildUpgradeItemModel(_modifierConfigs.Get(upgradeId)));
        }

        private MetaUpgradeItemModel BuildUpgradeItemModel(ParameterUpgradeConfig upgradeConfig)
        {
            var id = upgradeConfig.Id;
            var nextLevel = _upgradeService.GetNextLevel(id);
            var isMaxLevel = _upgradeService.IsMaxLevel(id);

            return new MetaUpgradeItemModel() {
                    Id = id,
                    Name = LocalizableText.Create(UPGRADE_NAME_LOCALIZATION_PREFIX + id),
                    Level = LocalizableText.Create(LEVEL_LOCALIZATION_ID, nextLevel),
                    IsMaxLevel = isMaxLevel,
                    PriceModel = CreatePriceModel(id, nextLevel, isMaxLevel),
                    OnClick = () => _onUpgrade?.Invoke(upgradeConfig.Id),
            };
        }

        private PriceButtonModel CreatePriceModel(string upgradeId, int nextLevel, bool isMaxLevel)
        {
            var productConfig = _shopService.GetProductById(upgradeId);
            var price = productConfig.GetFinalCost(nextLevel);
            return new PriceButtonModel() {
                    Price = price,
                    PriceText = price.ToString(),
                    Enabled = true,
                    CanBuy = _shopService.HasEnoughCurrencyAsObservable(upgradeId, nextLevel).Select(hasMoney => hasMoney && !isMaxLevel),
                    CurrencyIconPath = IconPath.GetCurrency(productConfig.ProductConfig.Currency.ToString())
            };
        }
    }
}