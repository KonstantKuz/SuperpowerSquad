﻿using YsoCorp.GameUtils;

namespace Survivors.ABTest.Providers
{
    public class YCABTestProvider : IABTestProvider
    {
        public string CurrentVariantId => YCManager.instance.abTestingManager.GetPlayerSample();
        public bool IsVariantId(string variantId) => YCManager.instance.abTestingManager.IsPlayerSample(variantId);
    }
}