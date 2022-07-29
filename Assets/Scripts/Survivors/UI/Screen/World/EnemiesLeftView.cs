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
            _waveGroupsSpawner.CurrentWaveIndex.Subscribe(UpdateWaveNumber).AddTo(_disposable);
        }

        private void OnKill(int killedCount)
        {
            var killRatio = (float) _waveGroupsSpawner.CurrentWaveKillCount / _waveGroupsSpawner.CurrentWaveCount;
            _progressView.SetData(killRatio == 1 ? 0 : killRatio);
        }

        private void UpdateWaveNumber(int waveIndex)
        {
            _text.SetTextFormatted(_text.LocalizationId, waveIndex + 1);
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