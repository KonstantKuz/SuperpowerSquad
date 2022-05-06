using UnityEngine;

namespace Feofun.UI
{
    public class SafeArea : MonoBehaviour
    {
        [SerializeField] private bool _enableTopSafeArea;
        [SerializeField] private bool _enableBottomSafeArea;
        private void Awake()
        {
            var rectTransform = GetComponent<RectTransform>();
            var safeArea = Screen.safeArea;
            if (_enableBottomSafeArea)
            {
                rectTransform.anchorMin = new Vector2(
                safeArea.xMin / Screen.width,
                safeArea.yMin / Screen.height);
            }

            if (_enableTopSafeArea)
            {
                rectTransform.anchorMax = new Vector2(
                    safeArea.xMax / Screen.width,
                    safeArea.yMax / Screen.height);
            }
        }
    }
}