using Feofun.UI.Components;
using Feofun.UI.Components.Button;
using Survivors.UI.Dialog.StartUnitDialog.Model;
using Survivors.Util;
using UnityEngine;
using UnityEngine.UI;

namespace Survivors.UI.Dialog.StartUnitDialog.View
{
    public class StartUnitItemView : MonoBehaviour
    {
    
        [SerializeField]
        private TextMeshProLocalization _name;
        
        [SerializeField] private Image _icon;
        [SerializeField]
        private ActionButton _button;
        
        
        public void Init(StartUnitItemModel model)
        {
            _name.SetTextFormatted(model.Name);
            _icon.sprite = Resources.Load<Sprite>(IconPath.GetUpgrade(model.Id));
            _button.Init(model.OnClick);
        }
    }
}