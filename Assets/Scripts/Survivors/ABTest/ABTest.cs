using JetBrains.Annotations;
using Logger.Extension;
using UnityEngine;

namespace Survivors.ABTest
{
    [PublicAPI]
    public class ABTest
    {
        public const string EXPERIMENT_ID = "version_1";
        
        private string _variantId;   
        
        public string CurrentVariantId => _variantId;
        public bool WithDisasters => CurrentVariantId.Equals(ABTestVariantId.WithDisasters.ToCamelCase());
        public ABTest()
        {
            Reload();
        }
        public void Reload()
        {
            _variantId = PlayerPrefs.GetString(GetKey(EXPERIMENT_ID), ABTestVariantId.Control.ToCamelCase());
            this.Logger().Info($"ABTest, setting ab test {EXPERIMENT_ID} to {_variantId}");
        }
        public static void SetVariantId(string variantId)
        {
            PlayerPrefs.SetString(GetKey(EXPERIMENT_ID), variantId);
        }
        private static string GetKey(string experimentId)
        {
            return $"ABTest_{experimentId}";
        }
    }
}