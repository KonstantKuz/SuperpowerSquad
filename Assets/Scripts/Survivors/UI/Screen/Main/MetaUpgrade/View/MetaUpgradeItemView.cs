using Feofun.UI.Components;
using Feofun.UI.Components.Button;
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
        private ActionButton _actionButton;
        [SerializeField]
        private GameObject _maxLevelContainer;
        [SerializeField]
        private GameObject _rewardedAdsContainer;

        private ActionButton Button => _actionButton;

        public void Init(MetaUpgradeItemModel model)
        {
            _name.SetTextFormatted(model.Name);
            _level.SetTextFormatted(model.Level);
            _icon.sprite = Resources.Load<Sprite>(IconPath.GetMetaUpgrade(model.Id));

            _maxLevelContainer.SetActive(model.State == UpgradeViewState.MaxLevel);
            _rewardedAdsContainer.SetActive(model.State == UpgradeViewState.CanBuyForAds);

            _priceView.Init(model.PriceModel);
            
            _actionButton.Init(() => model.OnClick?.Invoke(model));
            
            Button.Button.interactable = model.State != UpgradeViewState.MaxLevel;
        }
    }
}