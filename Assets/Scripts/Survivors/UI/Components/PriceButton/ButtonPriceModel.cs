using System;
using Survivors.Shop.Config;
using Survivors.Util;

namespace Survivors.UI.Components.PriceButton
{
    public class PriceButtonModel
    {
        public decimal Price;
        public string PriceText;
        public bool Enabled;
        public IObservable<bool> CanBuy;
        public string CurrencyIconPath;
        public bool ShowIcon => CurrencyIconPath != null;
        
        public static PriceButtonModel FromProduct(ProductConfig product, ShopService shop)
        {
            return new PriceButtonModel() {
                    Price = product.CurrencyCount,
                    PriceText = product.CurrencyCount.ToString(),
                    Enabled = true,
                    CanBuy = shop.HasEnoughCurrencyAsObservable(product.ProductId),
                    CurrencyIconPath = IconPath.GetCurrency(product.Currency.ToString())
            };
        }
    }
}