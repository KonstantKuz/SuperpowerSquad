using JetBrains.Annotations;
using Logger.Extension;
using UnityEngine;

namespace Survivors.ABTest
{
    [PublicAPI]
    public class ABTest
    {
        public const string TEST_ID = "version_1";
        
        private string _abTestId;   
        
        public string CurrentVariantId => _abTestId;
        public bool WithDisasters => CurrentVariantId.Equals(ABTestId.WithDisasters.ToCamelCase());
        public ABTest()
        {
            Reload();
        }
        public void Reload()
        {
            _abTestId = PlayerPrefs.GetString(GetKey(TEST_ID), ABTestId.Control.ToCamelCase());
            this.Logger().Info($"ABTest, setting ab test {TEST_ID} to {_abTestId}");
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