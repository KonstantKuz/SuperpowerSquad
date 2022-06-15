using Feofun.App.Init;

namespace Survivors.IOSTransparency
{
    public class IosATTInitStep: AppInitStep
    {
        private readonly IATTListener _attListener = new IosATTListener();
        protected override void Run()
        {
            _attListener.OnStatusReceived += OnATTStatusReceived;
            _attListener.Init();
        }
        
        private void OnATTStatusReceived()
        {
            _attListener.OnStatusReceived -= OnATTStatusReceived;
            Next();
        }
    }
}