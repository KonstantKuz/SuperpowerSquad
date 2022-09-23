using Feofun.Localization;
using JetBrains.Annotations;
using Survivors.WorldEvents.Config;

namespace Survivors.UI.Screen.World.WorldEvent
{
    public class AlertViewModel
    { 
        [CanBeNull]
        public LocalizableText Text { get; }
        public float ShowDuration { get; }
        public AlertViewModel(float showDuration, [CanBeNull] LocalizableText text = null)
        {
            Text = text;
            ShowDuration = showDuration;
        }

        public static AlertViewModel FromWorldEventType(WorldEventType eventType, float showDuration)
        {
            return new AlertViewModel(showDuration, LocalizableText.Create(eventType.ToString()));
        }
    }
}