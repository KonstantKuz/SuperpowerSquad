namespace Survivors.Session
{
    public interface IWorldScope
    {
        void OnWorldInit();
        void OnWorldCleanUp();
    }
}