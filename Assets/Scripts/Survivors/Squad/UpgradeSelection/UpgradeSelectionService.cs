using Survivors.Session;
using Survivors.Squad.Service;
using Survivors.Squad.Upgrade;
using Survivors.Squad.UpgradeSelection.Config;
using UniRx;
using Zenject;

namespace Survivors.Squad.UpgradeSelection
{
    public class UpgradeSelectionService : IWorldCleanUp
    {
        [Inject]
        private SquadProgressService _squadProgressService;       
        [Inject]
        private UpgradeService _upgradeService;      
        [Inject]
        private UpgradeSelectionConfig _upgradeSelectionConfig;
        
        private CompositeDisposable _disposable;


        public void Init()
        {
            _disposable?.Dispose();
            _disposable = new CompositeDisposable();
            _squadProgressService.LevelAsObservable.Subscribe(OnLevelUpgrade).AddTo(_disposable);
        }

        private void OnLevelUpgrade(int level)
        {
            if (level <= 1) {
                return;
            }
            CreateRandomUpgrades();
        }

        private void CreateRandomUpgrades()
        {
            
        }

        public void OnWorldCleanUp()
        {
            Dispose();
        }
        private void Dispose()
        {
            _disposable?.Dispose();
            _disposable = null;
        }
    }
}