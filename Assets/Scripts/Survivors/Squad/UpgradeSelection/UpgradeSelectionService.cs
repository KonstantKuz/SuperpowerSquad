using System.Collections.Generic;
using System.Linq;
using Feofun.Extension;
using Feofun.UI.Dialog;
using ModestTree;
using Survivors.Session;
using Survivors.Squad.Service;
using Survivors.Squad.Upgrade;
using Survivors.Squad.Upgrade.Config;
using Survivors.Squad.UpgradeSelection.Config;
using Survivors.UI.Dialog;
using UniRx;
using Zenject;

namespace Survivors.Squad.UpgradeSelection
{
    public class UpgradeSelectionService : IWorldCleanUp
    {
        private const int PROPOSED_UPGRADE_COUNT = 3;
      
        [Inject] private SquadProgressService _squadProgressService;
        [Inject] private UpgradeBranchSelectionConfig _upgradeSelectionConfig;
        [Inject] private UpgradesConfig _upgradesConfig;
        [Inject] private DialogManager _dialogManager;
        [Inject] private SquadUpgradeRepository _repository;
        
        private CompositeDisposable _disposable;
        private SquadUpgradeState SquadUpgradeState => _repository.Require();
        
        public void Init()
        {
            _disposable?.Dispose();
            _disposable = new CompositeDisposable();
            _squadProgressService.LevelAsObservable.Subscribe(OnSquadLevelUpgrade).AddTo(_disposable);
        }

        private void OnSquadLevelUpgrade(int level)
        {
            if (level <= 1) {
                return;
            }
            TryShowUpgradeDialog();
        }
        
        private void TryShowUpgradeDialog()
        {
            var randomUpgradeIds = GetRandomUpgradeIds(PROPOSED_UPGRADE_COUNT).ToList();
            if (randomUpgradeIds.IsEmpty()) {
                return;
            }
            _dialogManager.Show<UpgradeDialog, UpgradeDialogInitModel>(new UpgradeDialogInitModel(randomUpgradeIds));
        }

        private IEnumerable<string> GetRandomUpgradeIds(int upgradeCount)
        {
            var upgradeBranchIds = EnumExt.Values<UpgradeBranchType>().SelectMany(GetAvailableUpgradeBranchIds).ToList();
            return upgradeBranchIds.Count <= upgradeCount ? upgradeBranchIds : upgradeBranchIds.Random(upgradeCount);
        }

        private IEnumerable<string> GetAvailableUpgradeBranchIds(UpgradeBranchType branchType)
        {
            var upgradeBranchIds = _upgradesConfig.GetUpgradeBranchIds(branchType).ToList();
            var appliedBranchIds = upgradeBranchIds.Intersect(SquadUpgradeState.GetUpgradeBranchIds()).ToList();
            if (appliedBranchIds.Count >= _upgradeSelectionConfig.GetMaxUpgradeBranchCount(branchType)) {
                upgradeBranchIds = appliedBranchIds;
            }
            return upgradeBranchIds.Where(id => !SquadUpgradeState.IsMaxLevel(id, _upgradesConfig));
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