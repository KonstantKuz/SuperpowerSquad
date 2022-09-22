using Feofun.Components;

namespace Survivors.Units.Component.Hud
{
    public class SquadHudOwner : HudOwner, IInitializable<Squad.Squad>
    {
        public void Init(Squad.Squad owner)
        {
            CreateHud();
        }
    }
}