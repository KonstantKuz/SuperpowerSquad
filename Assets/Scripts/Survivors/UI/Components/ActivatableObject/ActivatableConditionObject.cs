using System.Linq;
using SuperMaxim.Core.Extensions;
using Survivors.UI.Components.ActivatableObject.Conditions;
using UniRx;
using UnityEngine;

namespace Survivors.UI.Components.ActivatableObject
{
    public class ActivatableConditionObject : MonoBehaviour
    {
        [SerializeField]
        private ActivatableObject[] _activatableObjects;

        private void Awake()
        {
            var conditions = GetComponents<ICondition>();
            var observer = conditions.Select(it => it.IsAllow()).CombineLatest().Select(it => it.All(b => b));
            _activatableObjects.ForEach(it => it.Init(observer));
        }
    }
}