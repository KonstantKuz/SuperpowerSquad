using System;
using UnityEngine;
using UnityEngine.UI;

namespace Feofun.UI.Components
{
    [RequireComponent(typeof(Button))]
    public class ActionButton : MonoBehaviour
    {
        private Action _action;
        private Button _button;

        public void Init(Action action)
        {
            _action = action;
        }
        private void Awake()
        {
            Button.onClick.AddListener(OnClick);
        }

        private void OnDestroy()
        {
            Button.onClick.RemoveListener(OnClick);
            _action = null;
        }

        private void OnClick()
        {
            _action?.Invoke();
        }

        public Button Button => _button ??= GetComponent<Button>();
    }
}