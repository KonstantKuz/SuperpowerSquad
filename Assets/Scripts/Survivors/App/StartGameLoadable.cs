using Feofun.App.Loadable;
using JetBrains.Annotations;

namespace Survivors.App
{
    [PublicAPI]
    public class StartGameLoadable : AppLoadable
    {
        public StartGameLoadable()
        {
        }

        protected override void Run()
        {
            Next();
        }
    }
}