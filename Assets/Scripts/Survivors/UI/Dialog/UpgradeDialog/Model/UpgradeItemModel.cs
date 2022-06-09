using System;

namespace Survivors.UI.Dialog.UpgradeDialog.Model
{
    public class UpgradeItemModel
    {
        public string Id;
        public string UpgradeName;
        public string UpgradeTypeName;     
        public string NextLevel;       
        public string Modifier;
        public Action OnClick;
    }
}