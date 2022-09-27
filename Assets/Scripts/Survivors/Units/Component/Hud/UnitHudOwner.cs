namespace Survivors.Units.Component.Hud
{
    public class UnitHudOwner : HudOwner<IUnit>
    {
        public override void Init(IUnit owner)
        {
            CreateHud();
        }
    }
}