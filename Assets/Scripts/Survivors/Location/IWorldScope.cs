namespace Survivors.Location
{
    public interface IWorldScope
    {
        void OnWorldSetup();
        void OnWorldCleanUp();
    }
}