namespace Survivors.Units.Component.Health
{
    public class UnitWithHealth : Health, IUnitInitializable
    {
        public void Init(IUnit unit)
        {
            base.Init(unit.Model.HealthModel);
        }
    }
}