using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Survivors.UI.Screen;
using UnityEngine;

namespace Feofun.UI.Screen
{
    public abstract class BaseScreen : MonoBehaviour
    {
        private const string METHOD_INIT_NAME = "Init";
        
        private List<MethodInfo> _initMethodInfos;
        private ScreenSwitcher SubSwitcher { get; set; }

        public string ScreenName => ScreenId.ToString();
        
        public abstract ScreenId ScreenId { get; }
        
        //Мне этот момент не нравится. Иерархия экранов дублируется в двух местах: в иерархии объектов сцены и здесь в Url экранов. 
        //Потенциальное место для ошибки. Надо будет перерпродумать.
        public abstract string Url { get; }
        protected virtual void Awake()
        {
            SubSwitcher = GetComponent<ScreenSwitcher>();
            _initMethodInfos = GetType().GetMethods().Where(it => it.Name == METHOD_INIT_NAME).ToList();
        }
        public void Show(params object[] initParams)
        {
            gameObject.SetActive(true);
            CallScreenInit(initParams);
        }
        public void CallScreenInit(params object[] initParams)
        {
            var initMethodInfo = _initMethodInfos.FirstOrDefault(m => m.GetParameters().Length == initParams.Length);
            if (initMethodInfo == null && initParams.Length == 0) {
                return;
            }
            if (initMethodInfo == null) {
                Debug.LogError($"CallScreenInit error, init method not found for params length, screen:= {GetType().Name}, params: {GetParamNames(initParams)}");
                return;
            }
            initMethodInfo.Invoke(this, initParams);
        }

        private string GetParamNames(object[] initParams)
        {
            return string.Join(" ", (initParams.Select(it => it.GetType() + ", ")).ToArray());
        }

        public virtual IEnumerator Hide()
        {
            if (SubSwitcher != null)
            {
                yield return SubSwitcher.HideActiveScreen();    
            }
            DeActivate();
        }

        public void DeActivate()
        {
            gameObject.SetActive(false);
        }
    }
}