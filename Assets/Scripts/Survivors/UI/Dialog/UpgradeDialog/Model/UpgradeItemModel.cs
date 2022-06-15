using System;
using Feofun.Localization;

namespace Survivors.UI.Dialog.UpgradeDialog.Model
{
    public class UpgradeItemModel
    {
        public string Id;
        public string Name;
        public LocalizableText Description;     
        public LocalizableText Level;
        public Action OnClick;
    }
}