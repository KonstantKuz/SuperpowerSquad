using Feofun.Util.SerializableDictionary;
using SuperMaxim.Core.Extensions;
using Survivors.UI.Dialog.UpgradeDialog.UpgradeKit.Model;
using Survivors.Util;
using UnityEngine;
using UnityEngine.UI;

namespace Survivors.UI.Dialog.UpgradeDialog.UpgradeKit.View
{
    public class UpgradeKitItemView : MonoBehaviour
    {
        [SerializeField]
        public Image _icon;

        [SerializeField]
        public SerializableDictionary<UpgradeKitViewState, GameObject> _stateContainers;

        public void Init(UpgradeKitItemModel model)
        {
            UpdateState(model);
            UpdateIcon(model);
        }
        private void UpdateIcon(UpgradeKitItemModel model)
        {
            if (model.Id != null) {
                _icon.sprite = Resources.Load<Sprite>(IconPath.GetUpgrade(model.Id));
                _icon.gameObject.SetActive(true);
            } else {
                _icon.gameObject.SetActive(false);
            }
        }

        private void UpdateState(UpgradeKitItemModel model)
        {
            _stateContainers.Values.ForEach(it => it.SetActive(false));
            _stateContainers[model.State].SetActive(true);
        }
    }
}