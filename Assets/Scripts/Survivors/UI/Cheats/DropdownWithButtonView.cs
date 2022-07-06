using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Survivors.UI.Cheats
{
    public class DropdownWithButtonView : MonoBehaviour
    {
        [SerializeField]
        private Dropdown _dropdown;
        [SerializeField]
        private Button _button;

        private Action<string> _onClick;

        public void Init(List<string> dropdownValues, Action<string> onClick)
        {
            _onClick = onClick;
            _dropdown.ClearOptions();
            _dropdown.AddOptions(dropdownValues);
        }

        private void OnEnable()
        {
            _button.onClick.AddListener(OnButtonClick);
        }

        private void OnDisable() => _button.onClick.RemoveListener(OnButtonClick);

        private void OnButtonClick()
        {
            _onClick?.Invoke(_dropdown.options[_dropdown.value].text);
        }
    }
}