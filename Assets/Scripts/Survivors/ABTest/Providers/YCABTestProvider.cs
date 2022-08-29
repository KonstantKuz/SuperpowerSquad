using System.Linq;
using Feofun.Extension;
using Logger.Extension;
using YsoCorp.GameUtils;

namespace Survivors.ABTest.Providers
{
    public class YCABTestProvider : IABTestProvider
    { 
        
        private readonly ABTest _abTest;
        
        public YCABTestProvider(ABTest abTest)
        {
            _abTest = abTest;
        }
        public void LoadAbTest()
        {
            foreach (var abTestId in EnumExt.Values<ABTestId>().Select(it => it.ToCamelCase())) {
                if (!IsVariantId(abTestId)) {
                    continue;
                }
                this.Logger().Info($"YCABTestProvider, experiment {ABTest.TEST_ID} value is {abTestId}");
                ABTest.SetExperiment(ABTest.TEST_ID, abTestId);
                _abTest.Reload();
                return;
            }
            this.Logger().Error($"YCABTestProvider hasn't set ab-test, defaul ab test: = {_abTest.CurrentVariantId}");
        }
        private bool IsVariantId(string variantId) => YCManager.instance.abTestingManager.IsPlayerSample(variantId);
    }
}