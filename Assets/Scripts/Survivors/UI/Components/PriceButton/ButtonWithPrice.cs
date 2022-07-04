using System;
using Feofun.UI.Components.Button;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Survivors.UI.Components.PriceButton
{
    public sealed class ButtonWithPrice : MonoBehaviour
    {
        [SerializeField] 
        private Text _priceText;
        [SerializeField]
        private ActionButton _actionButton; 
        [SerializeField]
        private Image _currencyIcon;
        [SerializeField]
        private Color _notEnoughCurrencyFontColor;
        
        private Color _defaultFontColor;
        private CompositeDisposable _disposable;
        
        public ActionButton Button => _actionButton;

        private void Awake()
        {
            _defaultFontColor = _priceText.color;
        }

        public void Init(PriceButtonModel model, Action action)
        {
            _disposable?.Dispose();
            _disposable = new CompositeDisposable();
            
            _actionButton.Init(action);
            Enabled = model.Enabled;
            if (!model.Enabled) {
                return;
            }
            PriceText = model.PriceText;
            model.CanBuy.Subscribe(SetСanBuyState).AddTo(_disposable);
            
            SetCurrencyActive(model.ShowIcon);
            if (model.ShowIcon) {
                SetIcon(model.CurrencyIconPath);
            }
        }
        private void SetСanBuyState(bool canBuy)
        {
            _actionButton.Button.interactable = canBuy;
            CurrencyColor = canBuy ? _defaultFontColor : _notEnoughCurrencyFontColor;
        }
        
        private void SetCurrencyActive(bool value)
        {
            _currencyIcon.gameObject.SetActive(value);
        }
        
        private void SetIcon(string iconPath)
        {
            _currencyIcon.sprite = Resources.Load<Sprite>(iconPath);
        }

        private Color CurrencyColor
        {
            set => _priceText.color = value;
        }
        private string PriceText
        {
            set => _priceText.text = value;
        }
        private bool Enabled
        {
            set => gameObject.SetActive(value);
        }

        private void OnDisable() => Dispose();
        private void OnDestroy() => Dispose();
        private void Dispose()
        {
            _disposable?.Dispose();
            _disposable = null;
        }
        
       
    }
}