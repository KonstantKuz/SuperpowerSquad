using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UniRx;
using UnityEngine;

namespace Survivors.Units.Component.Health
{
    [RequireComponent(typeof(Health))]
    public class DamageReaction : MonoBehaviour
    {
        private const string BaseColor = "_BaseColor";
        
        [SerializeField] private float _scalePunchForce;
        [SerializeField] private float _scalePunchDuration;
        [SerializeField] private float _colorBlinkDuration;
        [SerializeField] private Color _blinkColor;
        [SerializeField] private Renderer _renderer;

        private Health _health;
        private Color _startColor;
        private List<Tween> _currentTweens = new List<Tween>();
        
        private void Awake()
        {
            _health = gameObject.GetComponent<Health>();
            _health.OnDamageTaken += React;
            _health.OnDeath += Dispose;
            _startColor = _renderer.material.GetColor(BaseColor);
        }

        private void React()
        {
            StopCurrentTweens();

            PlayScalePunch();
            PlayColorBlink();
        }

        private void StopCurrentTweens()
        {
            _currentTweens.ForEach(it => it.Kill());
            _currentTweens.Clear();
        }

        private void PlayScalePunch()
        {
            var scaleTween = transform.DOPunchScale(Vector3.one * _scalePunchForce, _scalePunchDuration).SetEase(Ease.InOutQuad);
            _currentTweens.Add(scaleTween);
        }

        private void PlayColorBlink()
        {
            var toBlinkColor = _renderer.material.DOColor(_blinkColor, BaseColor, _colorBlinkDuration).SetEase(Ease.OutCubic);
            _currentTweens.Add(toBlinkColor);
            toBlinkColor.onComplete = () =>
            {
                var toOriginColor = _renderer.material.DOColor(_startColor, BaseColor, _colorBlinkDuration).SetEase(Ease.InCubic);
                _currentTweens.Add(toOriginColor);
            };
        }

        private void Dispose()
        {
            StopCurrentTweens();
            
            if (_health == null) return;
            
            _health.OnDamageTaken -= React;
            _health.OnDeath -= Dispose;
        }
    }
}
