using System;

namespace Survivors.Scope.Timer
{
    public interface IScopeTime
    { 
        bool IsPaused { get; } 
        float Time { get; }
        event Action OnTick;
    }
}