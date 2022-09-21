using System;
using System.Collections.Generic;
using Feofun.Localization;
using Survivors.UI.Dialog.UpgradeDialog.Star;

namespace Survivors.UI.Dialog.UpgradeDialog.Model
{
    public class UpgradeItemModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public LocalizableText Description { get; set; }
        public IEnumerable<StarViewModel> Stars { get; set; }
        public Action OnClick { get; set; }
    }
}