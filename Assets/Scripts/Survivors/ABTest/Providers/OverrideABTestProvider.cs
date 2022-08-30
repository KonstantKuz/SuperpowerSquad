using Survivors.Cheats;
using Zenject;

namespace Survivors.ABTest.Providers
{
    public class OverrideABTestProvider : IABTestProvider
    {
        private readonly IABTestProvider _impl;

        [Inject]
        private CheatsManager _cheatsManager;

        public OverrideABTestProvider(IABTestProvider impl)
        {
            _impl = impl;
        }
        public string GetVariant() => _cheatsManager.IsABTestCheatEnabled ? new CheatABTestProvider().GetVariant() : _impl.GetVariant();
    }
}
