using SuperMaxim.Messaging;
using Survivors.Player.Progress.Model;
using Survivors.Session.Messages;
using Survivors.Session.Model;
using UniRx;

namespace Survivors.Player.Progress.Service
{
    public class PlayerProgressService
    {
        private readonly PlayerProgressRepository _repository;
        
        private readonly IntReactiveProperty _gameCount; 
       
        public IReadOnlyReactiveProperty<int> GameCount => _gameCount;
        public PlayerProgress Progress => _repository.Get() ?? PlayerProgress.Create();

        public PlayerProgressService(IMessenger messenger, 
                                     PlayerProgressRepository repository)
        {
            _repository = repository;
            _gameCount = new IntReactiveProperty(Progress.GameCount);
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
            _gameCount.Value = progress.GameCount;
        }

        public void OnSessionStarted(int levelId)
        {
            var progress = Progress;
            progress.IncreasePassCount(levelId);
            SetProgress(progress);
        }

        public void AddKill()
        {
            var progress = Progress;
            progress.Kills++;
            SetProgress(progress);  //TODO: check that it's not very slow 
        }
    }
}