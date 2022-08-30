using System.Linq;
using Feofun.Extension;
using Logger.Extension;
using YsoCorp.GameUtils;

namespace Survivors.ABTest.Providers
{
    public class YCABTestProvider : IABTestProvider
    {
        public string GetVariant()
        {
            foreach (var variantId in EnumExt.Values<ABTestVariantId>().Select(it => it.ToCamelCase())) {
                if (!IsVariantId(variantId)) { //should check YCManager.IsPlayerSample(variantId), because YCManager.GetPlayerSample return "version - variantId"
                    continue;
                }
                this.Logger().Info($"YCABTestProvider, get variant ab-test, variant:= {variantId}");
                return variantId;
            }
            this.Logger().Error($"YCABTestProvider hasn't got ab-test variant, default ab-test variant:= {ABTestVariantId.Control}, YCManager ab-test variant:= {YCManager.instance.abTestingManager.GetPlayerSample()}");
            return ABTestVariantId.Control.ToCamelCase();
        }
        private bool IsVariantId(string variantId) => YCManager.instance.abTestingManager.IsPlayerSample(variantId); 
    }
}