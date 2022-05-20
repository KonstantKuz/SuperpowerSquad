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
            var safeArea = UnityEngine.Screen.safeArea;
            if (_enableBottomSafeArea)
            {
                rectTransform.anchorMin = new Vector2(
                safeArea.xMin / UnityEngine.Screen.width,
                safeArea.yMin / UnityEngine.Screen.height);
            }

            if (_enableTopSafeArea)
            {
                rectTransform.anchorMax = new Vector2(
                    safeArea.xMax / UnityEngine.Screen.width,
                    safeArea.yMax / UnityEngine.Screen.height);
            }
        }
    }
}