using Feofun.Config;
using Logger.Extension;
using Survivors.Config;
using Survivors.Modifiers.Config;
using Survivors.Shop.Service;
using Survivors.Squad.Upgrade;
using Survivors.UI.Screen.Main.MetaUpgrade.Model;
using Survivors.UI.Screen.Main.MetaUpgrade.View;
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

        private MetaUpgradeModel _model;

        private void OnEnable()
        {
            _model = new MetaUpgradeModel(_modifierConfigs, _upgradeService, _upgradeShopService, OnUpgrade);
            _view.Init(_model);
        }

        private void OnUpgrade(string upgradeId)
        {
            var level = _upgradeService.GetNextLevel(upgradeId);
            if (!_upgradeShopService.TryBuy(upgradeId, level)) {
                this.Logger().Error($"Can't buy meta upgrade, id:= {upgradeId}, upgrade level:={level}");
                return;
            }
            _upgradeService.Upgrade(upgradeId);
            _model.RebuildUpgradeItem(upgradeId);
        }

        private void OnDisable()
        {
            _model = null;
        }
    }
}