using Feofun.UI.Components;
using Survivors.UI.Components.PriceButton;
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
        private ButtonWithPrice _buttonWithPrice;     
        
        [SerializeField]
        private GameObject _maxLevelContainer;
        
        
        public void Init(MetaUpgradeItemModel model)
        {
            _name.SetTextFormatted(model.Name);
            _level.SetTextFormatted(model.Level);
            _icon.sprite = Resources.Load<Sprite>(IconPath.GetMetaUpgrade(model.Id));
            _maxLevelContainer.SetActive(model.IsMaxLevel);
            _buttonWithPrice.Init(model.PriceModel, model.OnClick);
            _buttonWithPrice.gameObject.SetActive(!model.IsMaxLevel);
        }
    }
}