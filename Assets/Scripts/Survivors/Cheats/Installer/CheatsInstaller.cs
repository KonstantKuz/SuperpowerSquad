using Feofun.Cheats;
using UnityEngine;
using Zenject;

namespace Survivors.Cheats.Installer
{
    public class CheatsInstaller : MonoBehaviour
    {
        [SerializeField] private CheatsManager _cheatsManager;
        [SerializeField] private CheatsActivator _cheatsActivator;
        
        public void Install(DiContainer container)
        {
            container.Bind<CheatsManager>().FromInstance(_cheatsManager).AsSingle();
            container.Bind<CheatsActivator>().FromInstance(_cheatsActivator).AsSingle();
        }
    }
}