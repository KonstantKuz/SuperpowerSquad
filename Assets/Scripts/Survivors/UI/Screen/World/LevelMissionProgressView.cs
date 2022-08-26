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
    public class LevelMissionProgressView : MonoBehaviour
    {
        [SerializeField] 
        private ProgressBarView _progressView;
        [SerializeField]
        private TextMeshProLocalization _text;

        [Inject] 
        private IMessenger _messenger;
        [Inject]
        private SessionService _sessionService;

        private CompositeDisposable _disposable;
        private void OnEnable()
        {
            Dispose();
            _disposable = new CompositeDisposable();
            _progressView.Reset(0);
            _messenger.Subscribe<SessionStartMessage>(Init);
        }

        private void Init(SessionStartMessage msg)
        {
            _text.SetTextFormatted(_text.LocalizationId, _sessionService.LevelConfig.Level);

            switch (_sessionService.LevelConfig.MissionType)
            {
                case LevelMissionType.KillCount:
                    _sessionService.Kills.Subscribe(OnKill).AddTo(_disposable);
                    break;
                case LevelMissionType.Time:
                    _sessionService.PlayTime.Subscribe(OnTick).AddTo(_disposable);
                    break;
                default:
                    throw new ArgumentOutOfRangeException($"Unexpected level mission type := {_sessionService.LevelConfig.MissionType}");
            }
        }

        private void OnKill(int killedCount)
        {
            _progressView.SetData((float) killedCount / _sessionService.LevelConfig.KillCount);
        }

        private void OnTick(float time)
        {
            _progressView.SetData(time / _sessionService.LevelConfig.Time);
        }

        private void Dispose()
        {
            _disposable?.Dispose();
            _disposable = null;
            _messenger.Unsubscribe<SessionStartMessage>(Init);
        }

        private void OnDisable()
        {
            Dispose();
        }
    }
}