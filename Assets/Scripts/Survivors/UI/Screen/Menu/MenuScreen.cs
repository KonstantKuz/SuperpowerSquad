using Feofun.UI.Screen;
using UnityEngine;

namespace Survivors.UI.Screen.Menu
{
    [RequireComponent(typeof(ScreenSwitcher))]
    public class MenuScreen : BaseScreen
    {
        public const ScreenId ID = ScreenId.Menu;
        public override ScreenId ScreenId => ID;
        
        public override string Url => ScreenName;
        
    }
}