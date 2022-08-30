using Logger.Extension;
using UnityEngine;

namespace Survivors.ABTest.Providers
{
    public class CheatABTestProvider : IABTestProvider
    {
        private const string CHEAT_KEY = "CheatAbTestId";
        
        public string GetVariant()
        {
            var variantId = GetVariantId();
            this.Logger().Info($"CheatABTestProvider, get variant ab-test, variant:= {variantId}"); 
            return variantId;
        }
        public static void SetVariantId(string variantId) => PlayerPrefs.SetString(CHEAT_KEY, variantId);
        private static string GetVariantId() => PlayerPrefs.GetString(CHEAT_KEY, ABTestVariantId.Control.ToCamelCase());
    }
}