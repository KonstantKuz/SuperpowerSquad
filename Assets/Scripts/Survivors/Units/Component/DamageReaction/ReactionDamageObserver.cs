using System;
using System.Collections.Generic;
using System.Linq;
using SuperMaxim.Core.Extensions;
using Survivors.Units.Component.DamageReaction.Reactions;
using Survivors.Units.Component.Health;
using UnityEngine;

namespace Survivors.Units.Component.DamageReaction
{
    [RequireComponent(typeof(IDamageable))]
    public class ReactionDamageObserver : MonoBehaviour
    {
        private IDamageable _damageable;

        private IEnumerable<IDamageReaction> _reactions;

        private void Awake()
        {
            _damageable = gameObject.GetComponent<IDamageable>();
            _reactions = gameObject.GetComponents<IDamageReaction>();
        }

        private void OnEnable()
        {
            _damageable.OnDamageTaken += OnDamageTakenReaction;
        }

        private void OnDamageTakenReaction(float damage)
        {
            if (gameObject == null) {
                return;
            }
            _reactions.ForEach(it => it.OnDamageReaction());
        }

        private void OnDisable() => Dispose();

        private void Dispose()
        {
            _reactions.OfType<IDisposable>().ForEach(it => it.Dispose());
            _damageable.OnDamageTaken -= OnDamageTakenReaction;
        }
    }
}