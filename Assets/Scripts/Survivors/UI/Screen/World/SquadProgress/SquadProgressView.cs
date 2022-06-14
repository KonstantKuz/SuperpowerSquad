using EasyButtons;
using Feofun.UI.Components;
using Survivors.Squad.Service;
using UniRx;
using UnityEngine;
using Zenject;

namespace Survivors.UI.Screen.World.SquadProgress
{
    public class SquadProgressView : MonoBehaviour
    {
        private const string LEVEL_LOCALIZATION_ID = "Level";
        
        [SerializeField] private ProgressBarView _expView;
        [SerializeField] private TextMeshProLocalization _level;
        
        [Inject]
        private SquadProgressService _squadProgressService;
        
        private SquadProgressModel _model;
        private CompositeDisposable _disposable;

        private void Awake()
        {
            _expView.IsIndependentUpdate = true;
        }
        private void OnEnable()
        {
            Dispose();
            _disposable = new CompositeDisposable();
            _model = new SquadProgressModel(_squadProgressService);
            _model.Level.Subscribe(it => _level.SetTextFormatted(LEVEL_LOCALIZATION_ID, it))
                  .AddTo(_disposable);
            _model.LevelProgress.Subscribe(it => { _expView.SetValueWithLoop(it); })
                  .AddTo(_disposable);
        }
        private void Dispose()
        {
            _disposable?.Dispose();
            _disposable = null;
            _model = null;
        }
        private void OnDisable()
        {
            Dispose();
        }

        [Button]
        public void AddExp(int amount)
        {
            _squadProgressService.AddExp(amount);
        }
    }
}