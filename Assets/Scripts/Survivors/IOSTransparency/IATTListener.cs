using System;

namespace Survivors.IOSTransparency
{
    public interface IATTListener
    { 
        event Action OnStatusReceived;
        void Init();
    }
}