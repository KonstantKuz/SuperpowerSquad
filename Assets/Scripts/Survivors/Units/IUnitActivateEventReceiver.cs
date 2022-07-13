namespace Survivors.Units
{
    public interface IUnitActivateEventReceiver
    {
        void OnActivate();
        void OnDeactivate();
    }
}