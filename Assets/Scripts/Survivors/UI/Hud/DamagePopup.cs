using System;
using DG.Tweening;
using Feofun.Extension;
using Survivors.App;
using TMPro;
using UnityEngine;
using Zenject;

namespace Survivors.UI.Hud
{
    public class DamagePopup : MonoBehaviour
    {
        [SerializeField]
        private float _lifeTime = 0.5f;
        [SerializeField]
        private float _maxScale = 1.5f;
        [SerializeField] 
        private float _maxHeight = 50f;
        [SerializeField] 
        private float _heightOffset = 5f;

        [SerializeField] 
        [Range(0f, 1f)] 
        private float _maxScaleTimeRatio = 0.5f;
        [SerializeField]
        [Range(0f, 1f)]
        private float _fadeTimeRatio = 0.3f;
        
        [SerializeField] private TextMeshProUGUI _text;
        
        private Vector3 _stayPosition;

        [Inject] private UpdateManager _updateManager;
        
        public void Init(string damage, Vector3 receiverPosition)
        {
            Dispose();
            _text.SetText(damage);
            _stayPosition = receiverPosition;
            _updateManager.StartUpdate(SetStayPosition);
        }

        public Tween PlayPopup()
        {
            var popupTween = DOTween.Sequence();
            var moveTween = _text.rectTransform.DOLocalMoveY(_maxHeight, _lifeTime);
            var scaleTween = _text.rectTransform.DOScale(Vector3.one * _maxScale, _lifeTime * _maxScaleTimeRatio);
            var zeroScaleTween = _text.DOFade(0f, _lifeTime * _fadeTimeRatio);
            popupTween.Join(moveTween).Join(scaleTween).Append(zeroScaleTween).Play();
            return popupTween;
        }

        private void SetStayPosition()
        {
            if (!_stayPosition.IsInViewport()) return;
            
            transform.position = UnityEngine.Camera.main.WorldToScreenPoint(_stayPosition) + Vector3.up * _heightOffset;
        }

        private void OnDisable()
        {
            Dispose();
        }

        private void Dispose()
        {
            _updateManager.StopUpdate(SetStayPosition);
            _text.rectTransform.localScale = Vector3.zero;
            var color = _text.color;
            color.a = 1;
            _text.color = color;
        }
    }
}