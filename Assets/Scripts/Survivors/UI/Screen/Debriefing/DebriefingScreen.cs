using Feofun.UI.Components.Button;
using Feofun.UI.Screen;
using JetBrains.Annotations;
using Survivors.Reward.Service;
using Survivors.Session.Model;
using Survivors.UI.Screen.Main;
using Survivors.UI.Screen.World;
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
        [Inject]
        private MissionResultRewardService _missionResultRewardService;  
        [Inject]
        private IRewardApplyService _rewardApplyService;

        [PublicAPI]
        public void Init(SessionResult result, Session.Model.Session session)
        {
            _winPanel.SetActive(result == SessionResult.Win);     
            _losePanel.SetActive(result == SessionResult.Lose); 
            _nextButton.gameObject.SetActive(result == SessionResult.Win);     
            _reloadButton.gameObject.SetActive(result == SessionResult.Lose);
            _rewardApplyService.ApplyRewards(_missionResultRewardService.CalculateRewards(result, session));
        }
        public void OnEnable()
        {
            _nextButton.Init(OnReload);
            _reloadButton.Init(OnReload);
        }

        private void OnReload()
        {
            _screenSwitcher.SwitchTo(MainScreen.ID.ToString());
        }
    }
}