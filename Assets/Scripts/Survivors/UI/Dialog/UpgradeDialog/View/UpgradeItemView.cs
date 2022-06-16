using Feofun.UI.Components;
using Feofun.UI.Components.Button;
using Survivors.UI.Dialog.UpgradeDialog.Model;
using Survivors.Util;
using UnityEngine;
using UnityEngine.UI;

namespace Survivors.UI.Dialog.UpgradeDialog.View
{
    public class UpgradeItemView : MonoBehaviour
    {
    
        [SerializeField]
        private TextMeshProLocalization _name; 
        [SerializeField]
        private TextMeshProLocalization _description;     
        [SerializeField]
        private TextMeshProLocalization _level;  
        [SerializeField] private Image _icon;
        [SerializeField]
        private ActionButton _button;
        
        
        public void Init(UpgradeItemModel model)
        {
            _name.LocalizationId = model.Name;
            _description.SetTextFormatted(model.Description);
            _level.SetTextFormatted(model.Level);
            _icon.sprite = Resources.Load<Sprite>(IconPath.GetUpgrade(model.Id));
            _button.Init(model.OnClick);
        }
    }
}