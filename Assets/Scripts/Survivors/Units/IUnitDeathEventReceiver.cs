namespace Survivors.Units
{
    public interface IUnitDeathEventReceiver
    {
        void OnDeath(DeathCause deathCause);
    }
}