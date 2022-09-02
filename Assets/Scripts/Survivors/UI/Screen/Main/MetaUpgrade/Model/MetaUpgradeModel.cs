using System;
using System.Collections.Generic;
using System.Linq;
using Feofun.Config;
using Feofun.Localization;
using SuperMaxim.Core.Extensions;
using Survivors.Modifiers.Config;
using Survivors.Shop.Service;
using Survivors.UI.Components.PriceView;
using Survivors.Upgrade.MetaUpgrade;
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

        private readonly Action<MetaUpgradeItemModel> _onUpgrade;
        private readonly List<ReactiveProperty<MetaUpgradeItemModel>> _upgrades;

        public IReadOnlyCollection<IObservable<MetaUpgradeItemModel>> Upgrades => _upgrades;

        public MetaUpgradeModel(StringKeyedConfigCollection<ParameterUpgradeConfig> modifierConfigs,
                                MetaUpgradeService upgradeService,
                                UpgradeShopService shopService,
                                Action<MetaUpgradeItemModel> onUpgrade)
        {
            _upgradeService = upgradeService;
            _shopService = shopService;
            _modifierConfigs = modifierConfigs;
            _onUpgrade = onUpgrade;
            _upgrades = modifierConfigs.Select(id => new ReactiveProperty<MetaUpgradeItemModel>(BuildUpgradeItemModel(id))).ToList();
        }

        public void Update()
        {
            _modifierConfigs.ForEach(it => { RebuildUpgradeItem(it.Id); });
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

            var state = GetState(upgradeConfig, nextLevel);

            return new MetaUpgradeItemModel() {
                    Id = id,
                    Name = LocalizableText.Create(UPGRADE_NAME_LOCALIZATION_PREFIX + id),
                    Level = LocalizableText.Create(LEVEL_LOCALIZATION_ID, nextLevel),
                    State = state,
                    PriceModel = CreatePriceModel(id, nextLevel, state == UpgradeViewState.CanBuyForCurrency),
                    OnClick = model => _onUpgrade?.Invoke(model),
            };
        }
        private UpgradeViewState GetState(ParameterUpgradeConfig upgradeConfig, int nextLevel)
        {
            var id = upgradeConfig.Id;
            if (_upgradeService.IsMaxLevel(id)) {
                return UpgradeViewState.MaxLevel;
            }
            if (!_upgradeService.IsPurchasedWithCurrency(id) && _shopService.HasEnoughCurrency(id, nextLevel)) {
                return UpgradeViewState.CanBuyForCurrency;
            }
            return UpgradeViewState.CanBuyForAds;
        }
        private PriceViewModel CreatePriceModel(string upgradeId, int nextLevel, bool enabled)
        {
            var productConfig = _shopService.GetProductById(upgradeId);
            var price = productConfig.GetFinalCost(nextLevel);
            return new PriceViewModel() {
                    Price = price,
                    PriceText = price.ToString(),
                    Enabled = enabled,
                    CanBuy = _shopService.HasEnoughCurrencyAsObservable(upgradeId, nextLevel),
                    CurrencyIconPath = IconPath.GetCurrency(productConfig.ProductConfig.Currency.ToString())
            };
        }
    }
}