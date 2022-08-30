using System;
using JetBrains.Annotations;
using Logger.Extension;
using Survivors.ABTest.Providers;
using Zenject;

namespace Survivors.ABTest
{
    [PublicAPI]
    public class ABTest
    {
        [Inject]
        private IABTestProvider _abTestProvider;
        
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
        public bool WithDisasters => CurrentVariantId.Equals(ABTestVariantId.WithDisasters.ToCamelCase());
        
        public void Reload()
        {
            _variantId = _abTestProvider.GetVariant();
            this.Logger().Info($"ABTest, setting ab-test variant:= {_variantId}");
        }
    }
}