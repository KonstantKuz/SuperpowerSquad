using Feofun.UI.Components;
using Feofun.UI.Screen;
using JetBrains.Annotations;
using Survivors.Session.Model;
using Survivors.UI.Screen.World;
using Survivors.Units;
using UnityEngine;
using Zenject;

namespace Survivors.UI.Screen.Debriefing
{
    public class DebriefingScreen : BaseScreen
    {
        public const ScreenId ID = ScreenId.Debriefing;
        public override ScreenId ScreenId => ID; 
        public override string Url => ScreenName;
        
        [SerializeField]
        private ActionButton _nextButton;    
        [SerializeField]
        private ActionButton _reloadButton;
        [SerializeField]
        private GameObject _winPanel;
        [SerializeField]
        private GameObject _losePanel;
        
        [Inject]
        private ScreenSwitcher _screenSwitcher;

        [PublicAPI]
        public void Init(SessionResult result)
        {
            _winPanel.SetActive(result == SessionResult.Win);     
            _losePanel.SetActive(result == SessionResult.Lose); 
            _nextButton.gameObject.SetActive(result == SessionResult.Win);     
            _reloadButton.gameObject.SetActive(result == SessionResult.Lose);
        }
        public void OnEnable()
        {
            _nextButton.Init(OnReload);
            _reloadButton.Init(OnReload);
        }

        private void OnReload()
        {
            _screenSwitcher.SwitchTo(WorldScreen.ID.ToString());
        }
    }
}