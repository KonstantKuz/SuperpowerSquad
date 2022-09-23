using DG.Tweening;
using Feofun.UI.Components;
using TMPro;
using UnityEngine;

namespace Survivors.UI.Screen.World.WorldEvent
{
    public class AlertView : MonoBehaviour
    {
        [SerializeField]
        private int _showCount;
        [SerializeField]
        private TextMeshProLocalization _textLocalization;
        
        private Sequence _textShowTween;
        private TMP_Text Text => _textLocalization.TextComponent;
        public void Init(AlertViewModel model)
        {
            Dispose();
            if (model.Text != null) {
                _textLocalization.SetTextFormatted(model.Text);
            }
            
            DisableText();
            gameObject.SetActive(true);
            
            PlayShowText(model);
        }

        private void DisableText()
        {
            var color = Text.color;
            color.a = 0;
            Text.color = color;
        }

        private void PlayShowText(AlertViewModel model)
        {
            var fadeDuration = model.ShowDuration / _showCount / 2;
            
            _textShowTween = DOTween.Sequence();
            for (int i = 0; i < _showCount; i++) {
                _textShowTween.Append(Text.DOFade(1, fadeDuration));
                _textShowTween.Append(Text.DOFade(0, fadeDuration));
            }
            _textShowTween.onComplete = () => {
                gameObject.SetActive(false);
            };
            _textShowTween.Play();
        
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