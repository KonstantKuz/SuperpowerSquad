using Zenject;

namespace Survivors.EnemySpawn
{
    public class EnemyServicesInstaller
    {
        public static void Install(DiContainer Container, 
            EnemyWavesSpawner enemyWavesSpawner)
        {
            Container.Bind<EnemyWavesSpawner>().FromInstance(enemyWavesSpawner);
        }
    }
}