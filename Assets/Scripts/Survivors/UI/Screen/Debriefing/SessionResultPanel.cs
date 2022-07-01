using UnityEngine;
using Feofun.UI.Components;
using Survivors.Session.Model;
using Survivors.UI.Screen.Debriefing.Model;

namespace Survivors.UI.Screen.Debriefing
{
    public class SessionResultPanel : MonoBehaviour
    {
        private const float MAX_LEVEL_NUMBER = 5f;
        [SerializeField] private GameObject _winPanel;
        [SerializeField] private GameObject _losePanel;
        [SerializeField] private AnimatedIntView _killCountView;
        [SerializeField] private AnimatedIntView _coinsCountView;
        [SerializeField] private ProgressBarView _unitProgressView;
        
        public void Init(ResultPanelModel model)
        {
            _winPanel.SetActive(model.SessionResult == SessionResult.Win);     
            _losePanel.SetActive(model.SessionResult == SessionResult.Lose); 
            _killCountView.SetData(model.KillCount);
            _coinsCountView.SetData(model.CoinsCount);
            var progress = model.CurrentLevel / MAX_LEVEL_NUMBER;
            _unitProgressView.SetData(progress);
        }
    }
}
