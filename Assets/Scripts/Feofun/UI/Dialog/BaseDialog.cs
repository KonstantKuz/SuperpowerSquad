﻿using UnityEngine;

namespace Feofun.UI.Dialog
{
    public abstract class BaseDialog : MonoBehaviour
    {
        public void Show()
        {
            gameObject.SetActive(true);
        }
        public void Hide()
        {
            DeActivate();
        }
        public void DeActivate()
        {
            gameObject.SetActive(false);
        }
    }
}