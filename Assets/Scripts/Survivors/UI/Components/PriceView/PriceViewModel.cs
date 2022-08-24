using System;

namespace Survivors.UI.Components.PriceView
{
    public class PriceViewModel
    {
        public decimal Price;
        public string PriceText;
        public bool Enabled;
        public IObservable<bool> CanBuy;
        public string CurrencyIconPath;
        public bool ShowIcon => CurrencyIconPath != null;
        
    }
}