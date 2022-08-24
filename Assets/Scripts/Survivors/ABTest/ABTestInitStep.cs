using Feofun.App.Init;
using Zenject;

namespace Survivors.ABTest
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