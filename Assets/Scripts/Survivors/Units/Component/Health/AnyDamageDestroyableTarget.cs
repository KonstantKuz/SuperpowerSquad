using System;
using Survivors.Extension;
using Survivors.Location.Model;
using Survivors.Units.Target;
using UnityEngine;

namespace Survivors.Units.Component.Health
{
    public class AnyDamageDestroyableTarget : MonoBehaviour, IDamageable, ITarget
    {
        [SerializeField] private UnitType _unitType;
        public bool DamageEnabled { get; set; }
        public string TargetId { get; }
        public UnitType UnitType => _unitType;
        public bool IsAlive { get; private set; } = true;
        public Transform Root => transform;
        public Transform Center => transform;
        public Action OnTargetInvalid { get; set; }
        public event Action OnZeroHealth = delegate { };
        public event Action OnDamageTaken = delegate { };

        public void TakeDamage(float damage, DamageUnits units = DamageUnits.Value)
        {
            Destroy(gameObject);
        }

        private void OnDestroy()
        {
            IsAlive = false;
            OnTargetInvalid?.Invoke();
        }
    }
}