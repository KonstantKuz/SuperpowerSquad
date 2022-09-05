using System;
using DG.Tweening;
using UnityEngine;

namespace Survivors.Units.Component.DamageReaction.Reactions
{
    public class DamageScaleReaction : MonoBehaviour, IDamageReaction, IDisposable
    {
        [SerializeField]
        private float _scalePunchForce;
        [SerializeField]
        private float _scalePunchDuration;

        private Tween _scalePunch;

        public void OnDamageReaction()
        {
            _scalePunch?.Kill(true);
            _scalePunch = transform.DOPunchScale(Vector3.one * _scalePunchForce, _scalePunchDuration).SetEase(Ease.InOutQuad);
        }

        public void Dispose()
        {
            _scalePunch?.Kill(true);
        }
    }
}