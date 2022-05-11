using Feofun.App.Init;
using JetBrains.Annotations;

namespace Survivors.App
{
    [PublicAPI]
    public class StartGameInitStep : AppInitStep
    {
        public StartGameInitStep()
        {
        }

        protected override void Run()
        {
            Next();
        }
    }
}