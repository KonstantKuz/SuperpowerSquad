using Feofun.UI.Components;
using Survivors.UI.Dialog.Model;
using TMPro;
using UnityEngine;

namespace Survivors.UI.Dialog.View
{
    public class UpgradeItemView : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI _type;     
        [SerializeField]
        private TextMeshProUGUI _name; 
        [SerializeField]
        private TextMeshProUGUI _nextLevel;     
        [SerializeField]
        private TextMeshProUGUI _modifier;      
        [SerializeField]
        private ActionButton _button;
        
        
        public void Init(UpgradeItemModel model)
        {
            _type.text += model.UpgradeTypeName;
            _name.text += model.UpgradeName;
            _nextLevel.text += model.NextLevel;
            _modifier.text += model.Modifier;
            _button.Init(model.OnClick);
        }
    }
}