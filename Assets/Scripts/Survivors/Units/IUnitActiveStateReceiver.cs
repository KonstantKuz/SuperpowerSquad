namespace Survivors.Units
{
    public interface IUnitActiveStateReceiver
    {
        void OnActiveStateChanged(bool active);
    }
}