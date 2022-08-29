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
            foreach (var variantId in EnumExt.Values<ABTestVariantId>().Select(it => it.ToCamelCase())) {
                if (!IsVariantId(variantId)) {
                    continue;
                }
                this.Logger().Info($"YCABTestProvider, experiment {ABTest.EXPERIMENT_ID} value is {variantId}");
                ABTest.SetVariantId(variantId);
                _abTest.Reload();
                return;
            }
            this.Logger().Error($"YCABTestProvider hasn't set ab-test, default ab-test variantId:= {_abTest.CurrentVariantId}, YCManager ab-test id:= {YCManager.instance.abTestingManager.GetPlayerSample()}");
        }
        private bool IsVariantId(string variantId) => YCManager.instance.abTestingManager.IsPlayerSample(variantId);
    }
}