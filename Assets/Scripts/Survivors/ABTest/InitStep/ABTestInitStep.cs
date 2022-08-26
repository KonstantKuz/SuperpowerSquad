using Feofun.App.Init;
using Survivors.ABTest.Providers;
using Zenject;

namespace Survivors.ABTest.InitStep
{
    public class ABTestInitStep : AppInitStep
    {
        [Inject] 
        private ABTest _abTest;

        protected override void Run()
        {
            _abTest.Provider = new YCABTestProvider();
            Next();
        }
    }
}