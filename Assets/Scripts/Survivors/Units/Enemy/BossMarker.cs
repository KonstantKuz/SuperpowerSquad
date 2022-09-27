using Feofun.Components;
using SuperMaxim.Messaging;
using Survivors.Units.Messages;
using UnityEngine;
using Zenject;

namespace Survivors.Units.Enemy
{
    public class BossMarker : MonoBehaviour, IInitializable<IUnit>
    {
        [Inject] private IMessenger _messenger;

        public void Init(IUnit owner)
        {
            _messenger.Publish(new BossSpawnedMessage(owner));
        }
    }
}