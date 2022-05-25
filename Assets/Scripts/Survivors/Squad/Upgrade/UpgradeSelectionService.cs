using Survivors.Session;
using Survivors.Squad.Service;
using UniRx;
using Zenject;

namespace Survivors.Squad.Upgrade
{
    public class UpgradeSelectionService : IWorldCleanUp
    {
        [Inject]
        private SquadProgressService _squadProgressService;
        
        private CompositeDisposable _disposable;


        private void Init()
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