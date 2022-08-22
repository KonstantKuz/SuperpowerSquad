using DG.Tweening;
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
        private TextMeshProUGUI _text;
        
        private Sequence _showTween;
        public void Init(EventViewModel model)
        {
            gameObject.SetActive(true);
            _showTween = DOTween.Sequence();
            var duration = _showDuration / _showCount / 2;
            for (int i = 0; i < _showCount; i++) {
                _showTween.Append(_text.DOFade(1, duration));
                _showTween.Append(_text.DOFade(0, duration));
            }
            _showTween.Play();
            _showTween.onComplete = () => {
                gameObject.SetActive(false);
            };
        }
    }
}