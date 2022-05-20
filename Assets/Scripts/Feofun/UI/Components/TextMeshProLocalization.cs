using Feofun.Localization;
using Feofun.Localization.Service;
using UnityEngine;
using Zenject;
using TMPro;

namespace Feofun.UI.Components
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class TextMeshProLocalization : MonoBehaviour
    {
        [Inject] private LocalizationService _localization;
        
        [SerializeField] private string _localizationId;

        private TMP_Text _text;
        private object[] _args;
        
        public string LocalizationId
        {
            get => _localizationId;
            set
            {
                _localizationId = value;
                _args = null;
                UpdateText();
            }
        }

        private void Awake()
        { 
            UpdateText();
        }
        public void SetTextFormatted(string localizationId, object[] args)
        {
            _localizationId = localizationId;
            _args = args; //not exactly correct - storing reference to mutable objects...
            UpdateText();
        }

        public void SetTextFormatted(string localizationId, object arg) => SetTextFormatted(localizationId, new[] { arg }); 
        public void SetTextFormatted(LocalizableText localizableText) => SetTextFormatted(localizableText.Id, localizableText.Args);
        private void UpdateText()
        {
            if (string.IsNullOrEmpty(_localizationId))
            {
                TextComponent.text = "";
                return;
            }

            if (_args == null || _args.Length == 0)
            {
                TextComponent.text = _localization.Get(_localizationId);
                return;
            }
            
            TextComponent.text = string.Format(_localization.Get(_localizationId), _args);
        }
        public TMP_Text TextComponent => _text ??= GetComponent<TextMeshProUGUI>();
        
    }
}