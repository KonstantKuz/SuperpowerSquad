using Feofun.UI.Components.Button;
using Feofun.UI.Screen;
using JetBrains.Annotations;
using Survivors.Reward.Service;
using Survivors.Session.Model;
using Survivors.UI.Screen.Main;
using Survivors.UI.Screen.Debriefing.Model;
using Survivors.UI.Screen.Menu;
using UnityEngine;
using Zenject;

namespace Survivors.UI.Screen.Debriefing
{
    public class DebriefingScreen : BaseScreen
    {
        public const ScreenId ID = ScreenId.Debriefing;
        public override ScreenId ScreenId => ID; 
        
        public static readonly string URL = MenuScreen.ID + "/" + ID;
        public override string Url => URL;

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

        [PublicAPI]
        public void Init(DebriefingScreenModel model)
        {
            _nextButton.gameObject.SetActive(model.SessionResult == SessionResult.Win);     
            _reloadButton.gameObject.SetActive(model.SessionResult == SessionResult.Lose);

            var rewards = _missionResultRewardService.CalculateRewards(model.SessionResult, model.Session);
            _rewardApplyService.ApplyRewards(rewards);

            var resultPanelModel = model.BuildResultPanelModel(rewards);
            _resultPanel.Init(resultPanelModel);
        }
        
        public void OnEnable()
        {
            _nextButton.Init(OnReload);
            _reloadButton.Init(OnReload);
        }

        private void OnReload()
        {
            _screenSwitcher.SwitchTo(MainScreen.URL);
        }
    }
}