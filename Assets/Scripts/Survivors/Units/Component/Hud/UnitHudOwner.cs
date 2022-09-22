using Feofun.Components;

namespace Survivors.Units.Component.Hud
{
    public class UnitHudOwner : HudOwner, IInitializable<IUnit>
    {
        public void Init(IUnit owner)
        {
            CreateHud();
        }
    }
}