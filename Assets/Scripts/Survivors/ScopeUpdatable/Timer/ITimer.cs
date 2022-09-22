using System;

namespace Survivors.ScopeUpdatable.Timer
{
    public interface ITimer
    { 
        bool Pause { get; } 
        float Time { get; } 
        float DeltaTime { get; }
        event Action OnUpdate;
    }
}