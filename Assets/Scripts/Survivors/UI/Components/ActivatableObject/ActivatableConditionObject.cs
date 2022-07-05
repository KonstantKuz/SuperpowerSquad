using System.Linq;
using Survivors.UI.Components.ActivatableObject.Conditions;
using UnityEngine;

namespace Survivors.UI.Components.ActivatableObject
{
    public class ActivatableConditionObject : MonoBehaviour
    {
        [SerializeField]
        private ActivatableObject[] _activatableObjects;

        private ICondition[] _conditions;

        private void Awake()
        {
            _conditions = GetComponents<ICondition>();
        }

        private void OnEnable()
        {
            var isAllow = _conditions.All(it => it.IsAllow());
            foreach (var activatableObject in _activatableObjects) {
                activatableObject.Init(isAllow);
            }
           
        }
    }
}