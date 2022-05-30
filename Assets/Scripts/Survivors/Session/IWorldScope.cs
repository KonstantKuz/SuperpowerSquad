namespace Survivors.Session
{
    public interface IWorldScope
    {
        void OnWorldSetup();
        void OnWorldCleanUp();
    }
}