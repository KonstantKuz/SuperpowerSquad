namespace Survivors.Units.Component.Hud
{
    public class SquadHudOwner : HudOwner<Squad.Squad>
    {
        public override void Init(Squad.Squad owner)
        {
            CreateHud();
        }
    }
}