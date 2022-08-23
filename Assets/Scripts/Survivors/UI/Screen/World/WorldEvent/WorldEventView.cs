using DG.Tweening;
using Feofun.UI.Components;
using TMPro;
using UnityEngine;

namespace Survivors.UI.Screen.World.WorldEvent
{
    public class WorldEventView : MonoBehaviour
    {
        [SerializeField]
        private float _showDuration;
        [SerializeField]
        private int _showCount;
        [SerializeField]
        private TextMeshProLocalization _textLocalization;
        
        private Sequence _textShowTween;
        
        private float FadeDuration => _showDuration / _showCount / 2;
        private TMP_Text Text => _textLocalization.TextComponent;
        public void Init(EventViewModel model)
        {
            Dispose();

            _textLocalization.SetTextFormatted(model.Text);
            DisableText();
            gameObject.SetActive(true);
            
            PlayShowText();
        }

        private void DisableText()
        {
            var color = Text.color;
            color.a = 0;
            Text.color = color;
        }

        private void PlayShowText()
        {
            _textShowTween = DOTween.Sequence();
            for (int i = 0; i < _showCount; i++) {
                _textShowTween.Append(Text.DOFade(1, FadeDuration));
                _textShowTween.Append(Text.DOFade(0, FadeDuration));
            }
            _textShowTween.Play();
            _textShowTween.onComplete = () => {
                gameObject.SetActive(false);
            };
        }

        private void OnDisable()
        {
            Dispose();
        }
        private void Dispose()
        {
            _textShowTween?.Kill();
            _textShowTween = null;
        }
    }
}