using Survivors.Squad.Upgrade;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Survivors.UI.Screen.World
{
    [RequireComponent(typeof(Button))]
    public class RandomUpgradeButton : MonoBehaviour
    {
        [Inject] private UpgradeService _upgradeService;
        private void Awake()
        {
            GetComponent<Button>().onClick.AddListener(AddRandomUpgrade);
        }

        private void AddRandomUpgrade()
        {
            _upgradeService.AddRandomUpgrade();
        }
    }
}