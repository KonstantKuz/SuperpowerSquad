using System;
using DG.Tweening;
using UnityEngine;

namespace Survivors.Units.Component.DamageReaction.Reactions
{
    public class DamageColorReaction : MonoBehaviour, IDamageReaction, IDisposable
    {
        
        [SerializeField]
        private float _colorBlinkDuration;
        [SerializeField]
        private Color _blinkColor;
        [SerializeField]
        private Renderer _renderer;
        [SerializeField]
        private int _materialIndex;
        
        private Color _startColor;
        private Tween _colorBlink;

        private void Awake()
        {
            _startColor = _renderer.materials[_materialIndex].color;
        }
        public void OnDamageReaction()
        {
            _colorBlink?.Kill(true);

            var toBlinkColor = DoColor(_blinkColor, Ease.OutCubic);
            var toOriginColor = DoColor(_startColor, Ease.InCubic);

            var sequence = DOTween.Sequence();
            _colorBlink = sequence.Append(toBlinkColor).Append(toOriginColor).Play();
        }

        private Tween DoColor(Color color, Ease ease)
        {
            return _renderer.materials[_materialIndex].DOColor(color, _colorBlinkDuration).SetEase(ease);
        }
        public void Dispose()
        { 
            _colorBlink?.Kill(true);
        }
    }
}