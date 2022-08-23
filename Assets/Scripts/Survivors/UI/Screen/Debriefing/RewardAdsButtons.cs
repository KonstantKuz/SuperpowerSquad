using System;
using Feofun.UI.Components.Button;
using UniRx;
using UnityEngine;

namespace Survivors.UI.Screen.Debriefing
{
    public class RewardAdsButtons : MonoBehaviour
    {
        [SerializeField] private ActionButton _acceptButton;
        [SerializeField] private ActionButton _declineButton;
        [SerializeField] private float _declineButtonShowDelay;

        private CompositeDisposable _disposable;
        
        public void Init(Action onAccepted, Action onDeclined)
        {
            Dispose();
            _disposable = new CompositeDisposable();
            
            _acceptButton.Init(onAccepted);
            _declineButton.Init(onDeclined);
            _declineButton.gameObject.SetActive(false);
            Observable.Timer(TimeSpan.FromSeconds(_declineButtonShowDelay)).Subscribe(it => ShowDeclineButton()).AddTo(_disposable);
        }

        private void ShowDeclineButton()
        {
            _declineButton.gameObject.SetActive(true);
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