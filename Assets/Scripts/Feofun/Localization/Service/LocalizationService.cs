using System.Collections.Generic;
using Feofun.Localization.Config;
using JetBrains.Annotations;
using SuperMaxim.Core.Extensions;
using UnityEngine;
using Zenject;

namespace Feofun.Localization.Service
{
    [PublicAPI]
    public class LocalizationService
    {
        private const string DEFAULT_LANGUAGE = "English";

        private readonly HashSet<string> _reportedUnsupportedLanguages = new HashSet<string>();

        [Inject] private LocalizationConfig _config;

        private string _languageOverride;
        
        public string Get(string localizationId)
        {
            if (!_config.Table.ContainsKey(localizationId))
            {
                Debug.LogWarning($"No localization for key: {localizationId}");
                return localizationId;
            }

            var localizations = _config.Table[localizationId];

            var language = Language;

            if (localizations.ContainsKey(language)) return _config.Table[localizationId][language];
            
            if (_reportedUnsupportedLanguages.Contains(language))
            {
                ReportUnsupportedLanguage(language);
            }
                
            return localizations[DEFAULT_LANGUAGE];
        }
        public void SetLanguageOverride(string language)
        {
            _languageOverride = language;
            PlayerPrefs.SetString("Language", language);
        }
        private void ReportUnsupportedLanguage(string language)
        {
            Debug.LogWarning($"Unsupported language: {language}");
            _reportedUnsupportedLanguages.Add(language);
        }
        private string LanguageOverride => _languageOverride ??= PlayerPrefs.GetString("Language", null);

        private string Language => LanguageOverride.IsNullOrEmpty() ? Application.systemLanguage.ToString() : LanguageOverride;

       
    }
}