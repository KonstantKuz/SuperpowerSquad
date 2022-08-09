using System.Collections;
using Feofun.App.Init;
using UnityEngine;
using YsoCorp.GameUtils;

namespace Survivors.App
{
    public class YsoCorpSDKInitStep: AppInitStep
    {
        private readonly MonoBehaviour _owner;
        
        public YsoCorpSDKInitStep(MonoBehaviour owner)
        {
            _owner = owner;
        }

        protected override void Run()
        {
            _owner.StartCoroutine(WaitForSDKInit());
        }

        private IEnumerator WaitForSDKInit()
        {
            while (YCManager.instance == null || !YCManager.instance.IsReady)
            {
                yield return null;
            }

            Next();
        }
    }
}