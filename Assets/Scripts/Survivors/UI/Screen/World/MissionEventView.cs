using Feofun.Extension;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

namespace Survivors.UI.Screen.World
{
    public class MissionEventView : MonoBehaviour
    {
        [SerializeField] private Image _iconPrefab;

        public void Init([CanBeNull]MissionEventModel model)
        {
            transform.DestroyAllChildren();
            CreateIcons(model);
        }

        private void CreateIcons(MissionEventModel model)
        {
            foreach (var ev in model.Events)
            {
                var icon = Instantiate(_iconPrefab, transform);
                icon.sprite = ev.Icon;
                var rectTransform = icon.transform as RectTransform;
                rectTransform.anchorMin = new Vector2(ev.Progress, rectTransform.anchorMin.y);
                rectTransform.anchorMax = new Vector2(ev.Progress, rectTransform.anchorMax.y);
            }
        }
    }
}