using Feofun.UI.Components;
using Feofun.UI.Dialog;
using Zenject;

namespace Survivors.UI.Dialog
{
    public class UpgradeDialog : BaseDialog, IUiInitializable<UpgradeDialogInitModel>
    {

        [Inject] private DialogManager _dialogManager;

        public void Init(UpgradeDialogInitModel initModel)
        {
           
        }
        private void OnCloseButton()
        {
            _dialogManager.Hide<UpgradeDialog>();
        }
        
    }
}