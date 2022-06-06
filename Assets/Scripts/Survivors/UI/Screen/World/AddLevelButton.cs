using Survivors.Squad.Service;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Survivors.UI.Screen.World
{
    [RequireComponent(typeof(Button))]
    public class AddLevelButton : MonoBehaviour
    {
        [Inject] private SquadProgressService _squadProgressService;
        
        private void Awake()
        {
            GetComponent<Button>().onClick.AddListener(AddExp);
        }

        private void AddExp()
        {
            _squadProgressService.AddExp(_squadProgressService.ExpToNextLevel);
        }
    }
}