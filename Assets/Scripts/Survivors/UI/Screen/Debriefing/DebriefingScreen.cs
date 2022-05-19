using Feofun.UI.Components;
using Feofun.UI.Screen;
using JetBrains.Annotations;
using Survivors.UI.Screen.World;
using Survivors.Units;
using UnityEngine;
using Zenject;

namespace Survivors.UI.Screen.Debriefing
{
    public class DebriefingScreen : BaseScreen
    {
        public const ScreenId DEBRIEFING_SCREEN = ScreenId.Debriefing;
        public override ScreenId ScreenId => DEBRIEFING_SCREEN; 
        public override string Url => ScreenName;
        
        [SerializeField]
        private ActionButton _nextButton;
        [Inject]
        private ScreenSwitcher _screenSwitcher;

        [PublicAPI]
        public void Init(UnitType winner)
        {
            
        }
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