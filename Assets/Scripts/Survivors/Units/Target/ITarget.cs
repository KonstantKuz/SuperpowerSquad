using System;
using UnityEngine;

namespace Survivors.Units.Target
{
    /// <summary>
    /// Для сущностей которые могут использоваться в качестве целей для атак, перемещения
    /// </summary>
    public interface ITarget
    {
        UnitType UnitType { get; set; }
        Action OnTargetInvalid { get; set; }
        Transform Root { get; }
        Transform Center { get; } 
        bool IsAlive { get; }
        string TargetId { get; }
        void OnDeath();
    }
}