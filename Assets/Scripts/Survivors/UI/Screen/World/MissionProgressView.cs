﻿using System;
using Feofun.UI.Components;
using SuperMaxim.Messaging;
using Survivors.Session.Config;
using Survivors.Session.Messages;
using Survivors.Session.Service;
using UniRx;
using UnityEngine;
using Zenject;

namespace Survivors.UI.Screen.World
{
    public class MissionProgressView : MonoBehaviour
    {
        [SerializeField] 
        private ProgressBarView _progressView;
        [SerializeField]
        private TextMeshProLocalization _text;

        private MissionProgressModel _model;
        private CompositeDisposable _disposable;

        public void Init(MissionProgressModel model)
        {
            Dispose();
            _model = model;
            _disposable = new CompositeDisposable();
            InitProgressBar();
            InitText();
        }

        private void InitProgressBar()
        {
            _progressView.Reset(0);
            _model.LevelProgress.Subscribe(UpdateProgress).AddTo(_disposable);
        }

        private void UpdateProgress(float progress)
        {
            _progressView.SetData(progress);
        }

        private void InitText()
        {
            _text.LocalizationId = _model.LabelId;
            _model.LabelContent.Subscribe(UpdateText).AddTo(_disposable);
        }

        private void UpdateText(string content)
        {
            _text.SetTextFormatted(_text.LocalizationId, content);
        }
        
        private void Dispose()
        {
            _disposable?.Dispose();
            _disposable = null;
        }

        private void OnDisable()
        {
            Dispose();
        }
    }
}