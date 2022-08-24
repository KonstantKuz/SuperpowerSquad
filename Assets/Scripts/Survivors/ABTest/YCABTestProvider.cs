﻿using YsoCorp.GameUtils;

namespace Survivors.ABTest
{
    public class YCABTestProvider: IABTestProvider
    { 
        public bool IsVariantId(string variantId) => YCManager.instance.abTestingManager.IsPlayerSample(variantId);
    }
}