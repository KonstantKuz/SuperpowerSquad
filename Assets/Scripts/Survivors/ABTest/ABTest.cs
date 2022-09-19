using System;
using JetBrains.Annotations;
using Logger.Extension;
using Survivors.ABTest.Providers;
using Survivors.App.Config;
using Zenject;

namespace Survivors.ABTest
{
    [PublicAPI]
    public class ABTest
    {
        [Inject]
        private IABTestProvider _abTestProvider;
        [Inject]
        private ConstantsConfig _constantsConfig;

        public bool IsEnabled => _constantsConfig.AbTestEnabled;
        
        private string _variantId;
        public string CurrentVariantId
        {
            get
            {
                if (_variantId == null) {
                    throw new NullReferenceException("ABTest not initialized, variantId is null");
                }
                return _variantId;
            }
        }
        public bool WithDisasters => IsEnabled && CurrentVariantId.Equals(ABTestVariantId.WithDisasters.ToCamelCase());
        
        public void Reload()
        {
            _variantId = _abTestProvider.GetVariant();
            this.Logger().Info($"ABTest, setting ab-test variant:= {_variantId}");
        }
    }
}