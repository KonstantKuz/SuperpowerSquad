using Feofun.Localization;
using Survivors.WorldEvents.Config;

namespace Survivors.UI.Screen.World.WorldEvent
{
    public class EventViewModel
    { 
        public LocalizableText Text { get; }
        public float ShowDuration { get; }
        public EventViewModel(WorldEventType eventType, float showDuration)
        {
            Text = LocalizableText.Create(eventType.ToString());
            ShowDuration = showDuration;
        }
    }
}