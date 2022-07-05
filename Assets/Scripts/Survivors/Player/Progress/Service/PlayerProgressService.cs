using SuperMaxim.Messaging;
using Survivors.Player.Progress.Model;
using Survivors.Session.Messages;
using Survivors.Session.Model;

namespace Survivors.Player.Progress.Service
{
    public class PlayerProgressService
    {
        private readonly PlayerProgressRepository _repository;
        
        public PlayerProgress Progress => _repository.Get() ?? PlayerProgress.Create();

        public PlayerProgressService(IMessenger messenger, 
                                     PlayerProgressRepository repository)
        {
            _repository = repository;
            messenger.Subscribe<SessionEndMessage>(OnSessionFinished);
        }

        private void OnSessionFinished(SessionEndMessage evn)
        {
            var progress = Progress;
            progress.GameCount++;
            if (evn.Result == SessionResult.Win) {
                progress.WinCount++;
            }
            SetProgress(progress);
        }
        
        private void SetProgress(PlayerProgress progress)
        {
            _repository.Set(progress);
        }

        public void OnSessionStarted(int levelId)
        {
            var progress = Progress;
            progress.IncreasePassCount(levelId);
            progress.Revives = 0;
            SetProgress(progress);
        }

        public void AddKill()
        {
            var progress = Progress;
            progress.Kills++;
            SetProgress(progress);  //TODO: check that it's not very slow 
        }

        public void AddRevive()
        {
            var progress = Progress;
            progress.Revives++;
            SetProgress(progress); 
        }
    }
}