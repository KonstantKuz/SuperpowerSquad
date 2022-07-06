using System;
using Feofun.Localization;
using Survivors.UI.Components.PriceButton;

namespace Survivors.UI.Screen.Main.MetaUpgrade.Model
{
    public class MetaUpgradeItemModel
    {
        public string Id;
        public LocalizableText Name;
        public LocalizableText Level;
        public bool IsMaxLevel;
        public PriceButtonModel PriceModel;
        public Action OnClick;
    }
}