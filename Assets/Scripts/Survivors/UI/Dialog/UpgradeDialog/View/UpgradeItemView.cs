using Feofun.Extension;
using Feofun.UI.Components;
using Feofun.UI.Components.Button;
using SuperMaxim.Core.Extensions;
using Survivors.UI.Dialog.UpgradeDialog.Model;
using Survivors.UI.Dialog.UpgradeDialog.Star;
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
        private Image _icon;
        [SerializeField]
        private ActionButton _button;
        [SerializeField]
        private StarView _starPrefab;
        [SerializeField]
        private Transform _starsRoot; 
    

        public void Init(UpgradeItemModel model)
        {
            Dispose();
            _name.LocalizationId = model.Name;
            _description.SetTextFormatted(model.Description);
            CreateStars(model);
            _icon.sprite = Resources.Load<Sprite>(IconPath.GetUpgrade(model.Id));
            _button.Init(model.OnClick);
        }

        private void CreateStars(UpgradeItemModel model)
        {
            model.Stars.ForEach(star => {
                var starView = Instantiate(_starPrefab, _starsRoot);
                starView.Init(star);
            });
            
        }
        private void OnDisable()
        { 
            _starsRoot.DestroyAllChildren();
        }

        private void Dispose()
        {
            _starsRoot.DestroyAllChildren();
        }
    }
}