using System;
using Feofun.UI.Components;
using Survivors.UI.Components;
using Survivors.UI.Components.PriceView;
using Survivors.UI.Screen.Main.MetaUpgrade.Model;
using Survivors.Util;
using UnityEngine;
using UnityEngine.UI;

namespace Survivors.UI.Screen.Main.MetaUpgrade.View
{
    public class MetaUpgradeItemView : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProLocalization _name;
        [SerializeField]
        private TextMeshProLocalization _level;
        [SerializeField]
        private Image _icon;
        [SerializeField]
        private PriceView _priceView;
        [SerializeField]
        private ButtonWithAds _button;
        [SerializeField]
        private GameObject _maxLevelContainer;
        [SerializeField]
        private GameObject _rewardedAdsContainer;

        public void Init(MetaUpgradeItemModel model)
        {
            _name.SetTextFormatted(model.Name);
            _level.SetTextFormatted(model.Level);
            _icon.sprite = Resources.Load<Sprite>(IconPath.GetMetaUpgrade(model.Id));

            _maxLevelContainer.SetActive(model.State == UpgradeViewState.MaxLevel);
            _rewardedAdsContainer.SetActive(model.State == UpgradeViewState.CanBuyForAds);

            _priceView.Init(model.PriceModel);
            
            InitButtonWithAds(model);
        }

        private void InitButtonWithAds(MetaUpgradeItemModel model)
        {
            _button.Init(() => model.OnClick?.Invoke(model));
            switch (model.State)
            {
                case UpgradeViewState.CanBuyForCurrency:
                    _button.SetOverride(model.PriceModel.Enabled);
                    break;
                case UpgradeViewState.CanBuyForAds:
                    _button.DeleteOverride();
                    break;
                case UpgradeViewState.MaxLevel:
                    _button.SetOverride(false);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}