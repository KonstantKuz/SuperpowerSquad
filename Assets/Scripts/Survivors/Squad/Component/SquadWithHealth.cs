using Survivors.Units.Component.Health;

namespace Survivors.Squad.Component
{
    public class SquadWithHealth : Health, ISquadInitializable
    {
        public void Init(Squad squad)
        {
            base.Init(squad.Model.HealthModel);
        }
    }
}