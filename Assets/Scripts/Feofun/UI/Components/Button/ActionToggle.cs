using System;
using UnityEngine;
using UnityEngine.UI;

namespace Feofun.UI.Components.Button
{
    [RequireComponent(typeof(Toggle))]
    public class ActionToggle : MonoBehaviour
    {
        private Action<bool> _action;
        private Toggle _toggle;

        public void Init(bool enabled, Action<bool> action)
        {
            Toggle.isOn = enabled;
            _action = action;
        }
            
        private void Awake()
        {
            Toggle.onValueChanged.AddListener(OnClick);
        }

        private void OnClick(bool enabled)
        {
            Toggle.isOn = enabled;
            _action?.Invoke(enabled);
        }

        private void OnDestroy()
        {
            Toggle.onValueChanged.RemoveListener(OnClick);
            _action = null;
        }

        private Toggle Toggle => _toggle ??= GetComponent<Toggle>();
    }
}