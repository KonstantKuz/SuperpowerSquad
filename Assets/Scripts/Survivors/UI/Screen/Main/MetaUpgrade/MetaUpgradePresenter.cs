using Feofun.Config;
using Logger.Extension;
using Survivors.Advertisment.Service;
using Survivors.Config;
using Survivors.Modifiers.Config;
using Survivors.Player.Wallet;
using Survivors.Shop.Service;
using Survivors.UI.Screen.Main.MetaUpgrade.Model;
using Survivors.UI.Screen.Main.MetaUpgrade.View;
using Survivors.Upgrade.MetaUpgrade;
using UniRx;
using UnityEngine;

using Zenject;

namespace Survivors.UI.Screen.Main.MetaUpgrade
{
    public class MetaUpgradePresenter : MonoBehaviour
    {
        [SerializeField]
        private MetaUpgradeView _view;

        [Inject(Id = Configs.META_UPGRADES)]
        private readonly StringKeyedConfigCollection<ParameterUpgradeConfig> _modifierConfigs;
        [Inject]
        private MetaUpgradeService _upgradeService;
        [Inject]
        private UpgradeShopService _upgradeShopService;  
        [Inject]
        private AdsManager _adsManager;       
        [Inject]
        private WalletService _walletService;
        
        private MetaUpgradeModel _model;
        
        private CompositeDisposable _disposable;

        private void OnEnable()
        {
            Dispose();
            _disposable = new CompositeDisposable();
            _model = new MetaUpgradeModel(_modifierConfigs, _upgradeService, _upgradeShopService, OnUpgrade);
            _view.Init(_model);
            _walletService.AnyMoneyObservable.Subscribe(it => _model.Update()).AddTo(_disposable);
        }

        private void OnUpgrade(MetaUpgradeItemModel model)
        {
            var upgradeId = model.Id;
            switch (model.State) {
                case UpgradeViewState.MaxLevel:
                    this.Logger().Error("Click on upgrade with max level");
                    return;
                case UpgradeViewState.CanBuyForCurrency:
                    BuyUpgrade(upgradeId);
                    return;
                default:
                    ShowRewardedAds(upgradeId);
                    break;
            }
        }

        private void ShowRewardedAds(string upgradeId)
        {
            if (_adsManager.IsRewardAdsReady()) {
                _adsManager.ShowRewardedAds(success => OnRewardedShown(success, upgradeId));
            } else {
                this.Logger().Warn($"Reward not ready, place:= {nameof(MetaUpgradePresenter)}");
            }
        }

        private void OnRewardedShown(bool success, string upgradeId)
        {
            if (success) {
                this.Logger().Info($"Rewarded ad is successful, place:= {nameof(MetaUpgradePresenter)}");
                _upgradeService.Upgrade(upgradeId);
                _model.RebuildUpgradeItem(upgradeId);
            } else {
                this.Logger().Warn($"Rewarded ad failed, place:= {nameof(MetaUpgradePresenter)}");
            }
        }

        private void BuyUpgrade(string upgradeId)
        {
            var level = _upgradeService.GetNextLevel(upgradeId);
            if (!_upgradeShopService.TryBuy(upgradeId, level)) {
                this.Logger().Error($"Can't buy meta upgrade, id:= {upgradeId}, upgrade level:={level}");
                return;
            }
            _upgradeService.Upgrade(upgradeId, true);
            _model.RebuildUpgradeItem(upgradeId);
        }

        private void Dispose()
        {
            _disposable?.Dispose();
            _disposable = null;
            _model = null;
        }

        private void OnDisable()
        {
            Dispose();
        }
    }
}