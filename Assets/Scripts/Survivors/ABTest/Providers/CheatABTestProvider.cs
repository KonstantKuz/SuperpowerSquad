
using Logger.Extension;
using UnityEngine;

namespace Survivors.ABTest.Providers
{
    public class CheatABTestProvider : IABTestProvider
    {
        private const string CHEAT_KEY = "CheatAbTestId";
        
        private readonly ABTest _abTest;
        
        public CheatABTestProvider(ABTest abTest)
        {
            _abTest = abTest;
        }
        public void LoadAbTest()
        {
            var variantId = GetVariantId();
            this.Logger().Info($"CheatABTestProvider, experiment {ABTest.EXPERIMENT_ID} value is {variantId}");
            ABTest.SetVariantId(variantId);
            _abTest.Reload();
        }
        public static void SetVariantId(string variantId) => PlayerPrefs.SetString(CHEAT_KEY, variantId);
        private static string GetVariantId() => PlayerPrefs.GetString(CHEAT_KEY, ABTestVariantId.Control.ToCamelCase());
    }
}