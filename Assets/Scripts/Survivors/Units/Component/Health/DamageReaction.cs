using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace Survivors.Units.Component.Health
{
    [RequireComponent(typeof(IDamageable))]
    public class DamageReaction : MonoBehaviour
    {
        private const string BASE_COLOR = "_BaseColor";
        
        [SerializeField] private float _scalePunchForce;
        [SerializeField] private float _scalePunchDuration;
        [SerializeField] private float _colorBlinkDuration;
        [SerializeField] private Color _blinkColor;
        [SerializeField] private Renderer _renderer;

        private IDamageable _damageable;
        private Color _startColor;
        private List<Tween> _currentTweens = new List<Tween>();
        
        private void Awake()
        {
            _damageable = gameObject.GetComponent<IDamageable>();
            _damageable.OnDamageTaken += React;
            _damageable.OnDeath += OnKilled;
            _startColor = _renderer.material.GetColor(BASE_COLOR);
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
            scaleTween.onComplete = () => _currentTweens.Remove(scaleTween);
        }

        private void PlayColorBlink()
        {
            var toBlinkColor = DoColor(_blinkColor, Ease.OutCubic);
            var toOriginColor = DoColor(_startColor, Ease.InCubic);

            var sequence = DOTween.Sequence();
            sequence.Append(toBlinkColor).Append(toOriginColor).Play();
            _currentTweens.Add(sequence);
            sequence.onComplete = () => { _currentTweens.Remove(sequence); };
        }

        private Tween DoColor(Color color, Ease ease)
        {
            return _renderer.material.DOColor(color, BASE_COLOR, _colorBlinkDuration).SetEase(ease);
        }

        private void OnKilled(DeathCause _) => Dispose();

        private void Dispose()
        {
            StopCurrentTweens();
            
            if (_damageable == null) return;
            
            _damageable.OnDamageTaken -= React;
            _damageable.OnDeath -= OnKilled;
        }

        private void OnDestroy()
        {
            Dispose();
        }
    }
}
