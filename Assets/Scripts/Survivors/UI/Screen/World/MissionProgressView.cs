using System;
using Feofun.UI.Components;
using SuperMaxim.Messaging;
using Survivors.Session.Config;
using Survivors.Session.Messages;
using Survivors.Session.Service;
using UniRx;
using UnityEngine;
using Zenject;

namespace Survivors.UI.Screen.World
{
    public class MissionProgressView : MonoBehaviour
    {
        [SerializeField] 
        private ProgressBarView _progressView;
        [SerializeField]
        private TextMeshProLocalization _text;

        [Inject]
        private SessionService _sessionService;

        private MissionProgressModel _model;
        private CompositeDisposable _disposable;

        public void Init(MissionProgressModel model)
        {
            Dispose();
            _model = model;
            _disposable = new CompositeDisposable();
            InitProgressBar();
            InitText();
        }

        private void InitProgressBar()
        {
            _progressView.Reset(0);
            _model.LevelProgress.Subscribe(UpdateProgress).AddTo(_disposable);
        }

        private void UpdateProgress(float progress)
        {
            _progressView.SetData(progress);
        }

        private void InitText()
        {
            _text.LocalizationId = _model.LabelId;
            switch (_model.MissionType)
            {
                case LevelMissionType.KillCount:
                    SetChapterNumber();
                    break;
                case LevelMissionType.Time:
                    SetSecondsToWin();
                    break;
                default:
                    throw new ArgumentOutOfRangeException($"Unexpected level mission type := {_model.MissionType}");
            }
        }

        private void SetChapterNumber()
        {
            _text.SetTextFormatted(_text.LocalizationId, _sessionService.LevelConfig.Level);
        }

        private void SetSecondsToWin()
        {
            _text.SetTextFormatted(_text.LocalizationId, _model.LeftSeconds);
            _model.LevelProgress.Subscribe(it => UpdateTimerText(_model.LeftSeconds)).AddTo(_disposable);
        }
        
        private void UpdateTimerText(float leftSeconds)
        {
            _text.SetTextFormatted(_text.LocalizationId, leftSeconds);
        }

        private void Dispose()
        {
            _disposable?.Dispose();
            _disposable = null;
        }

        private void OnDisable()
        {
            Dispose();
        }
    }
}