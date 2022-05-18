using Feofun.UI.Components;
using Feofun.UI.Screen;
using Survivors.UI.Screen.World;
using UnityEngine;
using Zenject;

namespace Survivors.UI.Screen.Debriefing
{
    public class DebriefingScreen : BaseScreen
    {
        private const ScreenId DEBRIEFING_SCREEN = ScreenId.Debriefing;
        public override ScreenId ScreenId => DEBRIEFING_SCREEN; 
        public override string Url => ScreenName;
        
        [SerializeField]
        private ActionButton _nextButton;
        [Inject]
        private ScreenSwitcher _screenSwitcher;
        
        public void OnEnable()
        {
            _nextButton.Init(OnReload);
        }

        private void OnReload()
        {
            _screenSwitcher.SwitchTo(WorldScreen.WORLD_SCREEN.ToString());
        }
    }
}