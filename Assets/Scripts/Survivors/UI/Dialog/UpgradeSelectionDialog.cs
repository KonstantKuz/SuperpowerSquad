using Feofun.UI.Components;
using Feofun.UI.Dialog;
using UnityEngine;
using Zenject;

namespace Survivors.UI.Dialog
{
    public class UpgradeSelectionDialog : BaseDialog, IUiInitializable<>
    {
        [SerializeField] private FootnoteView _footnoteView;

        [Inject] private DialogManager _dialogManager;

        public void Init(FootnoteInitModel initModel)
        {
            _footnoteView.Init(initModel, OnCloseButton);
        }
        private void OnCloseButton()
        {
            _dialogManager.Hide<UpgradeSelectionDialog>();
        }
        
    }
}