using System;

namespace Survivors.Scope.Timer
{
    public interface ITimer
    { 
        bool IsPaused { get; } 
        float Time { get; }
        event Action OnUpdate;
    }
}