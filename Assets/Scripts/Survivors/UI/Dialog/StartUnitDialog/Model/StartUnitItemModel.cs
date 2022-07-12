using System;
using Feofun.Localization;

namespace Survivors.UI.Dialog.StartUnitDialog.Model
{
    public class StartUnitItemModel
    {
        public string Id;
        public LocalizableText Name;
        public Action OnClick;
    }
}