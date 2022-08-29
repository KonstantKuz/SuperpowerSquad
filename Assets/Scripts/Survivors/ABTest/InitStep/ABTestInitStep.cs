using Feofun.App.Init;
using Survivors.ABTest.Providers;
using Survivors.Cheats;
using Zenject;

namespace Survivors.ABTest.InitStep
{
    public class ABTestInitStep : AppInitStep
    {
        [Inject] 
        private ABTest _abTest;        
        [Inject] 
        private CheatsManager _cheatsManager;
        protected override void Run()
        {
            var provider = (IABTestProvider) new YCABTestProvider(_abTest);
            if (_cheatsManager.IsABTestCheatEnabled) {
                provider = new CheatABTestProvider(_abTest);
            } 
            provider.LoadAbTest();
            Next();
        }
    }
}