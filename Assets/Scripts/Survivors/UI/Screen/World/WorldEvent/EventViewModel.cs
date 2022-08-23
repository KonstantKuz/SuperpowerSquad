using Feofun.Localization;
using Survivors.WorldEvents.Config;

namespace Survivors.UI.Screen.World.WorldEvent
{
    public class EventViewModel
    { 
        public LocalizableText Text { get; }
        public EventViewModel(WorldEventType eventType)
        {
            Text = LocalizableText.Create(eventType.ToString());
        }
    }
}