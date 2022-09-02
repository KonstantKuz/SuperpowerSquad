using Logger.Extension;
using Survivors.Cheats;
using UnityEngine;
using Zenject;

namespace Survivors.ABTest.Providers
{
    public class OverrideABTestProvider : IABTestProvider
    {
        private const string OVERRIDE_AB_TEST_KEY = "OverrideAbTestId";
        
        private readonly IABTestProvider _impl;

        [Inject]
        private CheatsManager _cheatsManager;

        public OverrideABTestProvider(IABTestProvider impl)
        {
            _impl = impl;
        }
        public string GetVariant() => _cheatsManager.IsABTestCheatEnabled ? GetOverrideVariant() : _impl.GetVariant();
        private string GetOverrideVariant()
        {
            var variantId = GetVariantFromPlayerPrefs();
            this.Logger().Info($"OverrideABTestProvider, get variant ab-test, variant:= {variantId}"); 
            return variantId;
        }
        public static void SetVariantId(string variantId) => PlayerPrefs.SetString(OVERRIDE_AB_TEST_KEY, variantId);
        private static string GetVariantFromPlayerPrefs() => PlayerPrefs.GetString(OVERRIDE_AB_TEST_KEY, ABTestVariantId.Control.ToCamelCase());
    
    }
}
