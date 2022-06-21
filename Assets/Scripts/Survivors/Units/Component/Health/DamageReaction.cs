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

        private Color _startColor;
        private IDamageable _damageable;
        
        private Tween _scalePunch;
        private Tween _colorBlink;
        private Sequence _jump;
        
        private void Awake()
        {
            _startColor = _renderer.material.GetColor(BASE_COLOR);
            _damageable = gameObject.GetComponent<IDamageable>();
            _damageable.OnDamageTaken += OnDamageTaken;
            _damageable.OnDeath += OnDeath;
        }

        public void PlayJump(float power, float duration)
        {
            _jump?.Kill(true);
            _jump = DOTween.Sequence();
            var initPosition = transform.position;
            var moveUp = transform.DOMove(initPosition + Vector3.up * power, duration).SetEase(Ease.OutExpo);
            var moveDown = transform.DOMove(initPosition, duration).SetEase(Ease.Linear);
            _jump.Append(moveUp).Append(moveDown).Play();
        }

        private void OnDamageTaken()
        {
            PlayScalePunch();
            PlayColorBlink();
        }

        private void PlayScalePunch()
        {
            _scalePunch?.Kill(true);
            _scalePunch = transform.DOPunchScale(Vector3.one * _scalePunchForce, _scalePunchDuration).SetEase(Ease.InOutQuad);
        }

        private void PlayColorBlink()
        {
            _colorBlink?.Kill(true);

            var toBlinkColor = DoColor(_blinkColor, Ease.OutCubic);
            var toOriginColor = DoColor(_startColor, Ease.InCubic);

            var sequence = DOTween.Sequence();
            _colorBlink = sequence.Append(toBlinkColor).Append(toOriginColor).Play();
        }

        private Tween DoColor(Color color, Ease ease)
        {
            return _renderer.material.DOColor(color, BASE_COLOR, _colorBlinkDuration).SetEase(ease);
        }

        private void OnDeath(DeathCause _) => Dispose();

        private void Dispose()
        {
            _scalePunch?.Kill(true); 
            _colorBlink?.Kill(true);
            _jump?.Kill(true); 

            if (_damageable == null) return;
            
            _damageable.OnDamageTaken -= OnDamageTaken;
            _damageable.OnDeath -= OnDeath;
        }

        private void OnDestroy()
        {
            Dispose();
        }
    }
}
