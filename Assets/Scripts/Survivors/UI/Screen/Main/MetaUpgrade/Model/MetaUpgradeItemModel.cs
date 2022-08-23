using System;
using Feofun.Localization;
using Survivors.UI.Components.PriceView;

namespace Survivors.UI.Screen.Main.MetaUpgrade.Model
{
    public class MetaUpgradeItemModel
    {
        public string Id;
        public LocalizableText Name;
        public LocalizableText Level;
        public PriceViewModel PriceModel;
        public UpgradeViewState State;
        public Action<MetaUpgradeItemModel> OnClick;
    }
}