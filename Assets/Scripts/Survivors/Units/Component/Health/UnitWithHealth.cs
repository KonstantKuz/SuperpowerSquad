namespace Survivors.Units.Component.Health
{
    public class UnitWithHealth : Health, IInitializable<IUnit>
    {
        public void Init(IUnit unit)
        {
            base.Init(unit.Model.HealthModel);
        }
    }
}