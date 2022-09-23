using Feofun.Localization;
using Survivors.WorldEvents.Config;

namespace Survivors.UI.Screen.World.WorldEvent
{
    public class AlertViewModel
    { 
        public LocalizableText Text { get; }
        public float ShowDuration { get; }
        public AlertViewModel(LocalizableText text, float showDuration)
        {
            Text = text;
            ShowDuration = showDuration;
        }

        public static AlertViewModel FromWorldEventType(WorldEventType eventType, float showDuration)
        {
            return new AlertViewModel(LocalizableText.Create(eventType.ToString()), showDuration);
        }
    }
}