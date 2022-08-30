using Feofun.App.Init;
using Zenject;

namespace Survivors.ABTest.InitStep
{
    public class ABTestInitStep : AppInitStep
    {
        [Inject] 
        private ABTest _abTest;        

        protected override void Run()
        {
            _abTest.Reload();
            Next();
        }
    }
}