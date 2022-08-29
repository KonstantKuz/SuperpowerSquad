
using Logger.Extension;
using UnityEngine;

namespace Survivors.ABTest.Providers
{
    public class CheatABTestProvider : IABTestProvider
    {
        private const string CHEAT_KEY = "CheatAbTestId";
        
        private readonly ABTest _abTest;
        
        public CheatABTestProvider(ABTest abTest)
        {
            _abTest = abTest;
        }
        public void LoadAbTest()
        {
            var cheatAbTestId = GetCheatAbTestId();
            this.Logger().Info($"CheatABTestProvider, experiment {ABTest.TEST_ID} value is {cheatAbTestId}");
            ABTest.SetExperiment(ABTest.TEST_ID, cheatAbTestId);
            _abTest.Reload();
        }
        public static void SetCheatAbTestId(string abTestId) => PlayerPrefs.SetString(CHEAT_KEY, abTestId);
        private static string GetCheatAbTestId() => PlayerPrefs.GetString(CHEAT_KEY, ABTestId.Control.ToCamelCase());
    }
}