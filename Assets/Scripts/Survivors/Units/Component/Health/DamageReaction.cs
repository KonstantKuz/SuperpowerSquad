using DG.Tweening;
using Survivors.Extension;
using UnityEngine;

namespace Survivors.Units.Component.Health
{
    [RequireComponent(typeof(Unit))]
    [RequireComponent(typeof(IDamageable))]
    public class DamageReaction : MonoBehaviour
    {
        private const string BASE_COLOR = "_BaseColor";
        
        [SerializeField] private float _scalePunchForce;
        [SerializeField] private float _scalePunchDuration;
        [SerializeField] private float _colorBlinkDuration;
        [SerializeField] private Color _blinkColor;
        [SerializeField] private Renderer _renderer;

        private Unit _owner;
        private Color _startColor;
        private IDamageable _damageable;
        
        private Tween _scalePunch;
        private Tween _colorBlink;
        private Sequence _jump;
        
        private void Awake()
        {
            _owner = gameObject.RequireComponent<Unit>();
            _damageable = gameObject.GetComponent<IDamageable>();
            _startColor = _renderer.material.GetColor(BASE_COLOR);
            _damageable.OnDamageTaken += OnDamageTakenReact;
            _damageable.OnDeath += OnDeath;
        }

        public void ExplosionReact(Vector3 explosionPosition, float jumpForce, float jumpHeight, float jumpDuration)
        {
            if(!_owner.IsActive) { return; }
            
            _jump = DOTween.Sequence();
            var jumpDirection = transform.position - explosionPosition;
            var jumpPosition = transform.position + jumpForce * Vector3.ProjectOnPlane(jumpDirection, Vector3.up) /  jumpDirection.magnitude;
            var jumpMove = transform.DOJump(jumpPosition, jumpHeight, 1, jumpDuration);
            var rotateToExplosion = transform.DORotateQuaternion(Quaternion.LookRotation(jumpDirection), jumpDuration / 2);
            var rotateBack = transform.DORotateQuaternion(Quaternion.Euler(Vector3.zero), jumpDuration / 2);
            _jump.Append(jumpMove);
            _jump.Insert(0, rotateToExplosion).Insert(jumpDuration / 2, rotateBack);
            _jump.Play();
            
            _owner.IsActive = false;
            _jump.onComplete += () =>
            {
                _owner.IsActive = true;
            };
        }

        private void OnDamageTakenReact()
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
            
            _damageable.OnDamageTaken -= OnDamageTakenReact;
            _damageable.OnDeath -= OnDeath;
        }

        private void OnDestroy()
        {
            Dispose();
        }
    }
}
