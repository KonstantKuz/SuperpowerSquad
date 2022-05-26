using System;
using Survivors.Squad.Upgrade;
using Survivors.Squad.UpgradeSelection;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Survivors.UI.Screen.World
{
    [RequireComponent(typeof(Button))]
    public class RandomUpgradeButton : MonoBehaviour
    {
        [Inject] private UpgradeSelectionService _upgradeService;
        private void Awake()
        {
            GetComponent<Button>().onClick.AddListener(AddRandomUpgrade);
        }

        private void AddRandomUpgrade()
        {
            _upgradeService.TryShowUpgradeDialog();
        }
    }
}