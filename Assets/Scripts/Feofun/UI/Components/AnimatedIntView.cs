using DG.Tweening;
using TMPro;
using UnityEngine;

namespace Feofun.UI.Components
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class AnimatedIntView : AnimatedValueView<int, TextMeshProUGUI>
    {
        public override int Value
        {
            get => int.TryParse(Component.text, out var value) ? value : 0;
            protected set => Component.text = value.ToString();
        }

        protected override Tweener Animate(int fromValue, int toValue, float time) =>
                DOTween.To(() => fromValue, value => { Value = value; }, toValue, time);
    }
}