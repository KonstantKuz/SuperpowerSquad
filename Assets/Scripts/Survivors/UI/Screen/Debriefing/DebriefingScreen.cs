using Feofun.UI.Components.Button;
using Feofun.UI.Screen;
using JetBrains.Annotations;
using Survivors.Reward.Service;
using Survivors.Session.Model;
using Survivors.UI.Screen.Main;
using Survivors.Session.Service;
using Survivors.UI.Screen.Debriefing.Model;
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
        private SessionResultPanel _resultPanel;
        [SerializeField]
        private ActionButton _nextButton;    
        [SerializeField]
        private ActionButton _reloadButton;

        [Inject]
        private ScreenSwitcher _screenSwitcher;       
        [Inject]
        private MissionResultRewardService _missionResultRewardService;  
        [Inject]
        private IRewardApplyService _rewardApplyService;
        [Inject]
        private SessionService _sessionService;
        
        [PublicAPI]
        public void Init(DebriefingScreenModel model)
        {
            _nextButton.gameObject.SetActive(model.SessionResult == SessionResult.Win);     
            _reloadButton.gameObject.SetActive(model.SessionResult == SessionResult.Lose);

            var rewards = _missionResultRewardService.CalculateRewards(model.SessionResult, model.Session);
            _rewardApplyService.ApplyRewards(rewards);

            var resultPanelModel = model.BuildResultPanelModel(rewards, _sessionService.LevelId);
            _resultPanel.Init(resultPanelModel);
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