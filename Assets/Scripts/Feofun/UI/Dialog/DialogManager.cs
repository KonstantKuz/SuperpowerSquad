using System;
using System.Collections.Generic;
using System.Linq;
using Feofun.Extension;
using Feofun.UI.Components;
using SuperMaxim.Core.Extensions;
using UnityEngine;

namespace Feofun.UI.Dialog
{
    public class DialogManager : MonoBehaviour
    {
        private List<BaseDialog> _dialogs;

        private readonly List<BaseDialog> _activeDialogs = new List<BaseDialog>();

        private void Awake()
        {
            _dialogs = GetComponentsInChildren<BaseDialog>(true).ToList();
            DeActivateAll();
        }

        private void DeActivateAll()
        {
            _dialogs.ForEach(it => it.Hide());
        }
        public TDialog Show<TDialog>() where TDialog : BaseDialog
        {
            var dialog = GetDialog<TDialog>();
            if (_activeDialogs.Contains(dialog)) {
                Hide<TDialog>();
            }
            AddActiveDialog(dialog);
            dialog.Show();
            return dialog as TDialog;
        }
        public void Show<TDialog, TParam>(TParam initParam)
                where TDialog : BaseDialog, IUiInitializable<TParam>
        {
            var dialog = GetDialog<TDialog>();
            Show<TDialog>();
            IUiInitializable<TParam> uiInitializable = (TDialog) dialog;
            uiInitializable.Init(initParam);
        }
        public void Hide<TDialog>() where TDialog : BaseDialog
        {
            var dialog = GetDialog<TDialog>();
            if (!_activeDialogs.Contains(dialog)) {
                return;
            }
            dialog.Hide();
            _activeDialogs.Remove(dialog);
        }

        public bool IsDialogActive<TDialog>()
                where TDialog : BaseDialog
        {
            return _activeDialogs.Contains(GetDialog<TDialog>());
        }
        public void HideAll()
        {
            _activeDialogs.ForEach(it => it.Hide());
            _activeDialogs.Clear();
        }

        private void AddActiveDialog(BaseDialog dialog)
        {
            _activeDialogs.Add(dialog);
            Sort();
        }
        private void Sort()
        {
            _activeDialogs.ForEach(it => { it.GetComponent<RectTransform>().SetSiblingIndex(_activeDialogs.IndexOf(it)); });
        }

        private BaseDialog GetDialog<TDialog>() =>
                _dialogs.FirstOrDefault(it => it.GetType() == typeof(TDialog))
                ?? throw new ArgumentException($"Trying to get to non-existing dialog {typeof(TDialog).Name}");
    }
}