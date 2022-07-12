using System;
using Feofun.UI.Components;
using Survivors.Session.Service;
using UniRx;
using UnityEngine;
using Zenject;

namespace Survivors.UI.Screen.World
{
    public class EnemiesLeftView : MonoBehaviour
    {
        [SerializeField] 
        private ProgressBarView _progressView;
        [SerializeField]
        private TextMeshProLocalization _text;

        [Inject]
        private SessionService _sessionService;

        private CompositeDisposable _disposable;
        private void OnEnable()
        {
            Dispose();
            _disposable = new CompositeDisposable();
            _progressView.Reset(0);
            _sessionService.Kills.Subscribe(OnKill).AddTo(_disposable);
            _text.SetTextFormatted(_text.LocalizationId, _sessionService.LevelConfig.Level);
        }

        private void OnKill(int killedCount)
        {
            _progressView.SetData((float) killedCount / _sessionService.LevelConfig.KillCount);
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