using Feofun.Util.SerializableDictionary;
using SuperMaxim.Core.Extensions;
using UnityEngine;

namespace Survivors.UI.Dialog.UpgradeDialog.Star
{
    public class StarView : MonoBehaviour
    {

        [SerializeField]
        public SerializableDictionary<StarState, GameObject> _stateContainers;
        
        public void Init(StarViewModel model)
        {
            UpdateState(model);
        }
        private void UpdateState(StarViewModel model)
        {
            _stateContainers.Values.ForEach(it => it.SetActive(false));
            _stateContainers[model.State].SetActive(true);
        }
    }
}