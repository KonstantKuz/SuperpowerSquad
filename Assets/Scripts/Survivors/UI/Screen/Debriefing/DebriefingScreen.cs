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
        [SerializeField]
        private GameObject _winPanel;
        [SerializeField]
        private GameObject _losePanel;
        
        [Inject]
        private ScreenSwitcher _screenSwitcher;

        [PublicAPI]
        public void Init(UnitType winner)
        {
            _winPanel.SetActive(winner == UnitType.PLAYER);     
            _losePanel.SetActive(winner != UnitType.PLAYER);
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