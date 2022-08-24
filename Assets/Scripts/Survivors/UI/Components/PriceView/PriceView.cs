using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Survivors.UI.Components.PriceView
{
    public sealed class PriceView : MonoBehaviour
    {
        [SerializeField] 
        private TextMeshProUGUI _priceText;
        [SerializeField]
        private Image _currencyIcon;
        [SerializeField]
        private Color _notEnoughCurrencyFontColor;
        
        private Color _defaultFontColor;
        
        private CompositeDisposable _disposable;

        private void Awake()
        {
            _defaultFontColor = _priceText.color;
        }

        public void Init(PriceViewModel model)
        {
            _disposable?.Dispose();
            _disposable = new CompositeDisposable();
            
            Enabled = model.Enabled;
            if (!model.Enabled) {
                return;
            }
            PriceText = model.PriceText;
            model.CanBuy.Subscribe(SetCanBuyState).AddTo(_disposable);
            
            SetCurrencyActive(model.ShowIcon);
            if (model.ShowIcon) {
                SetIcon(model.CurrencyIconPath);
            }
        }
        private void SetCanBuyState(bool canBuy)
        {
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