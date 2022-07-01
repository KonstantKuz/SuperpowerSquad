using EasyButtons;
using Feofun.UI.Components;
using Feofun.Util.SerializableDictionary;
using Survivors.Player.Wallet;
using UniRx;
using UnityEngine;
using Zenject;

namespace Survivors.UI.Screen.Header
{
    public class WalletWidgetPresenter : MonoBehaviour
    {
        [SerializeField]
        private SerializableDictionary<Currency, AnimatedIntView> _moneyViews;

        [Inject]
        private WalletService _walletService;

        private CompositeDisposable _disposable;

        private void OnEnable()
        {
            _disposable?.Dispose();
            _disposable = new CompositeDisposable();

            foreach (var pair in _moneyViews) {
                _walletService.GetMoneyAsObservable(pair.Key).Subscribe(it => pair.Value.SetData(it)).AddTo(_disposable);
            }
        }

        private void OnDisable()
        {
            _disposable?.Dispose();
            _disposable = null;
        }

        [Button("Add Currency", Mode = ButtonMode.EnabledInPlayMode)]
        public void AddCurrency(Currency currency, int amount)
        {
            _walletService.Add(currency, amount);
        }
    }
}