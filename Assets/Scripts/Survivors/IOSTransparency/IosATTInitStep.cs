using Feofun.App.Init;

namespace Survivors.IOSTransparency
{
    public class IosATTInitStep: AppInitStep
    {
        private readonly IATTListener _attListener = new IosATTListener();
        protected override void Run()
        {
#if UNITY_IOS 
            _attListener.OnStatusReceived += OnATTStatusReceived;
            _attListener.Init();
#else
            Next();
#endif            
        }
        
        private void OnATTStatusReceived()
        {
            _attListener.OnStatusReceived -= OnATTStatusReceived;
            Next();
        }
    }
}