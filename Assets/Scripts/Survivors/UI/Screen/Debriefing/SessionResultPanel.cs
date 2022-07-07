using System.Collections;
using UnityEngine;
using Feofun.UI.Components;
using Survivors.Session.Model;
using Survivors.UI.Screen.Debriefing.Model;

namespace Survivors.UI.Screen.Debriefing
{
    public class SessionResultPanel : MonoBehaviour
    {
        private const float PROGRESS_LEVELS_COUNT = 4f;

        [SerializeField] private float _animateValuesDelay;
        [SerializeField] private GameObject _winPanel;
        [SerializeField] private GameObject _losePanel;
        [SerializeField] private AnimatedIntView _killCountText;
        [SerializeField] private AnimatedIntView _coinsCountText;
        [SerializeField] private ProgressBarView _unitProgressView;
        [SerializeField] private AnimatedIntView _unitProgressText;

        private ResultPanelModel _model;
        private Coroutine _showStatisticsCoroutine;

        public void Init(ResultPanelModel model)
        {
            _model = model;
            _winPanel.SetActive(model.SessionResult == SessionResult.Win);     
            _losePanel.SetActive(model.SessionResult == SessionResult.Lose);
            ResetStatistics();
            ResetUnitProgress();
            StopStatisticAnimation();
            _showStatisticsCoroutine = StartCoroutine(ShowStatisticWithDelay());
        }

        private void ResetStatistics()
        {
            _killCountText.Reset(0);
            _coinsCountText.Reset(0);
        }

        private void ResetUnitProgress()
        {
            _unitProgressText.Reset();
            _unitProgressView.Reset();
            
            var previousUnitProgress = GetLevel() / PROGRESS_LEVELS_COUNT;
            InitUnitProgressView(previousUnitProgress);
        }

        private IEnumerator ShowStatisticWithDelay()
        {
            yield return new WaitForSeconds(_animateValuesDelay);
            InitStatistics(_model.KillCount, _model.CoinsCount);
            var unitProgress = (GetLevel() + 1) / PROGRESS_LEVELS_COUNT;
            InitUnitProgressView(unitProgress);
            _showStatisticsCoroutine = null;
        }
        private float GetLevel() => Mathf.Clamp(_model.CurrentLevel - 1, 0, PROGRESS_LEVELS_COUNT);
        private void InitStatistics(int killCount, int coinsCount)
        {
            _killCountText.SetData(killCount);
            _coinsCountText.SetData(coinsCount);
        }

        private void InitUnitProgressView(float barProgress)
        {
            _unitProgressView.SetData(barProgress);
            var textProgress = barProgress * 100f;
            _unitProgressText.SetData((int) textProgress);
        }

        private void OnDisable()
        {
            StopStatisticAnimation();
        }

        private void StopStatisticAnimation()
        {
            if (_showStatisticsCoroutine == null) return;
            StopCoroutine(_showStatisticsCoroutine);
            _showStatisticsCoroutine = null;
        }
    }
}
