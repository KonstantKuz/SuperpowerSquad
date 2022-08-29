using System;
using JetBrains.Annotations;
using Survivors.ABTest.Providers;

namespace Survivors.ABTest
{
    [PublicAPI]
    public class ABTest
    {
        private const string CONTROL_VARIANT_ID = "control";
        private const string WITH_DISASTERS_VARIANT_ID = "withDisasters";

        private IABTestProvider _provider;
        
        public IABTestProvider Provider
        {
            get
            {
                if (_provider == null) {
                    throw new NullReferenceException("ABTestProvider is null, ABTest is not ready yet");
                }
                return _provider;
            }
            set => _provider = value;
        }

        public string CurrentVariantId => Provider.CurrentVariantId;
        public bool Control => Provider.IsVariantId(CONTROL_VARIANT_ID);
        public bool WithDisasters => Provider.IsVariantId(WITH_DISASTERS_VARIANT_ID);
    }
}