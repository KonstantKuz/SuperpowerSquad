using System;

namespace Survivors.UI.Dialog.Model
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