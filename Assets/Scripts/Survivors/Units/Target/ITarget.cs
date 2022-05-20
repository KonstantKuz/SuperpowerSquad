using System;
using UnityEngine;

namespace Survivors.Units.Target
{
    /// <summary>
    /// Для сущностей которые могут использоваться в качестве целей для атак, перемещения
    /// </summary>
    public interface ITarget
    {
        string TargetId { get; }
        UnitType UnitType { get; }
        bool IsAlive { get; }
        Transform Root { get; }
        Transform Center { get; } 
        Action OnTargetInvalid { get; set; }
    }
}