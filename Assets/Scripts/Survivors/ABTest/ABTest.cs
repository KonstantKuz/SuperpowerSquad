using JetBrains.Annotations;
using Logger.Extension;
using UnityEngine;

namespace Survivors.ABTest
{
    [PublicAPI]
    public class ABTest
    {
        public const string TEST_ID = "version_1";
        
        private string _variantId;   
        
        public string CurrentVariantId => _variantId;
        public bool WithDisasters => CurrentVariantId.Equals(ABTestVariantId.WithDisasters.ToCamelCase());
        public ABTest()
        {
            Reload();
        }
        public void Reload()
        {
            _variantId = PlayerPrefs.GetString(GetKey(TEST_ID), ABTestVariantId.Control.ToCamelCase());
            this.Logger().Info($"ABTest, setting ab test {TEST_ID} to {_variantId}");
        }
        public static void SetExperiment(string experimentId, string variantId)
        {
            PlayerPrefs.SetString(GetKey(experimentId), variantId);
        }
        private static string GetKey(string experimentId)
        {
            return $"ABTest_{experimentId}";
        }
    }
}