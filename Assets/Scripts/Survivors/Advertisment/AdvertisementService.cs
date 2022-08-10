using SuperMaxim.Messaging;
using Survivors.Session.Messages;
using Survivors.Session.Model;

namespace Survivors.Advertisement
{
    public class AdvertisementService
    {
        public AdvertisementService(IMessenger messenger)
        {
            messenger.Subscribe<SessionStartMessage>(OnSessionStart);
            messenger.Subscribe<SessionEndMessage>(OnSessionEnd);
        }

        private void OnSessionEnd(SessionEndMessage msg)
        {
            YsoCorp.GameUtils.YCManager.instance.OnGameFinished( msg.Result == SessionResult.Win);
        }

        private void OnSessionStart(SessionStartMessage msg)
        {
            YsoCorp.GameUtils.YCManager.instance.OnGameStarted(msg.Level);
        }
    }
}