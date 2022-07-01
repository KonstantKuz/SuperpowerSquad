using System.Collections;
using UnityEngine;
using Feofun.UI.Components;
using Survivors.Session.Model;
using Survivors.UI.Screen.Debriefing.Model;
using TMPro;

namespace Survivors.UI.Screen.Debriefing
{
    public class SessionResultPanel : MonoBehaviour
    {
        private const float PROGRESS_LEVELS_COUNT = 5f;

        [SerializeField] private float _animateValuesDelay;
        [SerializeField] private GameObject _winPanel;
        [SerializeField] private GameObject _losePanel;
        [SerializeField] private AnimatedIntView _killCountText;
        [SerializeField] private AnimatedIntView _coinsCountText;
        [SerializeField] private ProgressBarView _unitProgressView;
        [SerializeField] private AnimatedIntView _unitProgressText;

        private ResultPanelModel _model;

        public void Init(ResultPanelModel model)
        {
            _model = model;
            _winPanel.SetActive(model.SessionResult == SessionResult.Win);     
            _losePanel.SetActive(model.SessionResult == SessionResult.Lose);
            ResetStatistics();
            ResetUnitProgress();
            StartCoroutine(DelayedAnimateStatistics());
        }

        private void ResetStatistics()
        {
            _killCountText.Reset();
            _killCountText.SetData(0);
            _coinsCountText.Reset();
            _coinsCountText.SetData(0);
        }

        private void ResetUnitProgress()
        {
            _unitProgressText.Reset();
            _unitProgressView.Reset();

            var previousLevel = Mathf.Clamp(_model.CurrentLevel - 1, 0, PROGRESS_LEVELS_COUNT);
            var previousUnitProgress = previousLevel / PROGRESS_LEVELS_COUNT;
            InitUnitProgressView(previousUnitProgress);
        }

        private IEnumerator DelayedAnimateStatistics()
        {
            yield return new WaitForSeconds(_animateValuesDelay);
            InitStatistics(_model.KillCount, _model.CoinsCount);
            var currentUnitProgress = _model.CurrentLevel / PROGRESS_LEVELS_COUNT;
            InitUnitProgressView(currentUnitProgress);
        }

        private void InitStatistics(int killCount, int coinsCount)
        {
            _killCountText.SetData(killCount);
            _coinsCountText.SetData(coinsCount);
        }

        private void InitUnitProgressView(float barProgress)
        {
            _unitProgressView.IsIndependentUpdate = true;
            _unitProgressView.SetData(barProgress);
            var textProgress = barProgress * 100f;
            _unitProgressText.SetData((int) textProgress);
        }
    }
}
