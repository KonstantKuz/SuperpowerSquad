using System;
using System.Linq;
using Feofun.UI.Components;
using SuperMaxim.Messaging;
using Survivors.Enemy.Spawn;
using Survivors.Session.Messages;
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
        private IMessenger _messenger;
        [Inject]
        private WaveGroupsSpawner _waveGroupsSpawner;
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
            _messenger.Subscribe<WaveClearedMessage>(ResetEnemiesLeft);
        }

        private void ResetEnemiesLeft(WaveClearedMessage msg)
        {
            _progressView.Reset(0);
        }

        private void OnKill(int killedCount)
        {
            _progressView.SetData((float) killedCount / _sessionService.LevelConfig.Waves.Sum(it => it.Count));
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