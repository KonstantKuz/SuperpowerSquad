using Feofun.Extension;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

namespace Survivors.UI.Screen.World.Mission
{
    public class MissionEventView : MonoBehaviour
    {
        [SerializeField] private Image _iconPrefab;

        public void Init([CanBeNull]MissionEventModel model)
        {
            transform.DestroyAllChildren();
            model?.Events.ForEach(CreateIcon);
        }

        private void CreateIcon(MissionEventModel.MissionEvent missionEvent)
        {
            var icon = Instantiate(_iconPrefab, transform);
            icon.sprite = missionEvent.Icon;
            var rectTransform = icon.transform as RectTransform;
            rectTransform.anchorMin = new Vector2(missionEvent.Progress, rectTransform.anchorMin.y);
            rectTransform.anchorMax = new Vector2(missionEvent.Progress, rectTransform.anchorMax.y);
        }
    }
}